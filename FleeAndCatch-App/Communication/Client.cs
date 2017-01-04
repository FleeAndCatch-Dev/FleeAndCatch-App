using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;
using Commands.Components;
using Commands.Devices;
using Commands.Devices.Apps;
using Commands.Identifications;
using Newtonsoft.Json.Linq;
using Sockets.Plugin;
using static Commands.Components.ComponentType;

namespace Communication
{
    public static class Client
    {
        private static TcpSocketClient tcpSocketClient;
        private static bool connected;
        private static ClientIdentification identification;
        private static Device device;

        /// <summary>
        /// Create a new task with a new communication.
        /// </summary>
        public static void Connect(string pAddress)
        {
            ParseAddress(pAddress);

            tcpSocketClient = new TcpSocketClient();
            identification = new ClientIdentification(0, IdentificationType.App.ToString(), pAddress, Default.Port);
            device = new App(new AppIdentification(0, IdentificationType.App.ToString(), RoleType.Undefined.ToString()));
            connected = false;

            if (connected) throw new Exception("Connection to the server is already exist");
            var listenTask = new Task(Listen);
            listenTask.Start();
        }

        /// <summary>
        /// Listen at the current socket and interpret the received commands.
        /// </summary>
        private static async void Listen()
        {
            await tcpSocketClient.ConnectAsync(identification.Address, identification.Port);
            connected = true;

            var command = new Connection(CommandType.Connection.ToString(), ConnectionType.Connect.ToString(), identification, device);
            SendCmd(command.GetCommand());

            while (connected)
            {
                Interpreter.Parse(ReceiveCmd());
            }
        }

        /// <summary>
        /// Receive a command from the current socket.
        /// </summary>
        /// <returns>String: Json command</returns>
        private static string ReceiveCmd()
        {
            var size = new byte[4];

            tcpSocketClient.ReadStream.Read(size, 0, size.Length);

            var length = size.Select((t, i) => (int) (t*Math.Pow(128, i))).Sum();
            var data = new byte[length];
            tcpSocketClient.ReadStream.Read(data, 0, data.Length);

            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// Send command to the current socket.
        /// </summary>
        /// <param name="pCommand">Json command</param>
        public static async void SendCmd(string pCommand)
        {
            if (!connected) throw new Exception("There is no connection to the server");
            checkCmd(pCommand);

            var command = Encoding.UTF8.GetBytes(pCommand);
            var size = new byte[4];
            var rest = pCommand.Length;
            for (var i = 0; i < size.Length; i++)
            {
                size[size.Length - (i + 1)] = (byte) (rest/Math.Pow(128, size.Length - (i + 1)));
                rest = (int) (rest%Math.Pow(128, size.Length - (i + 1)));
            }
                
            tcpSocketClient.WriteStream.Write(size, 0, size.Length);
            await tcpSocketClient.WriteStream.FlushAsync();

            tcpSocketClient.WriteStream.Write(command, 0, command.Length);
            await tcpSocketClient.WriteStream.FlushAsync();
        }

        /// <summary>
        /// Disconnect the current device an dispose all ressources
        /// </summary>
        public static async void Disconnect()
        {
            connected = false;
            await tcpSocketClient.DisconnectAsync();
            tcpSocketClient.Dispose();
        }

        /// <summary>
        /// Close the current connection, for start of the closing.
        /// </summary>
        public static void Close()
        {
            if (!Connected) throw new Exception("There is no connection to the server");
            var command = new Connection(CommandType.Connection.ToString(), ConnectionType.Disconnect.ToString(), identification, device);
            SendCmd(command.GetCommand());
        }

        /// <summary>
        /// Parse the current address of an ip address.
        /// </summary>
        /// <param name="pAddress">Connection address to the server.</param>
        private static void ParseAddress(string pAddress)
        {
            var adressPart = pAddress.Split(new string[] {"."}, StringSplitOptions.None);
            if (adressPart.Length != 4) throw new Exception("The Address could not parse into an ip adress");
            var result = true;
            foreach (var t in adressPart)
            {
                int value;
                if (!int.TryParse(t, out value) && value >= 0 && value <= 255)
                    result = false;
            }
            if (result)
                return;
            throw new Exception("The Address could not parse into an ip adress");
        }

        /// <summary>
        /// Check the current command of a json command.
        /// </summary>
        /// <param name="pCommand">Json command</param>
        private static void checkCmd(string pCommand)
        {
            try
            {
                JObject.Parse(pCommand);
            }
            catch (Exception)
            {
                throw new Exception("The command could not parse into json");
            }
        }

        public static bool Connected => connected;
        public static ClientIdentification Identification => identification;
        public static Device Device => Device;
    }
}
