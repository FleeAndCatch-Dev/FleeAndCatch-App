using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;
using Newtonsoft.Json.Linq;
using Sockets.Plugin;

namespace Communication
{
    public static class Client
    {
        private static TcpSocketClient TcpSocketClient;
        private static string Address;
        private static int Port;
        private static string type = ComponentType.ClientType.Type.App.ToString();
        private static string subtype = "null";

        public static int Id { get; set; }
        public static bool Connected { get; set; }

        /// <summary>
        /// Create a new task with a new communication.
        /// </summary>
        public static void Connect(string pAddress)
        {
            ParseAddress(pAddress);

            TcpSocketClient = new TcpSocketClient();
            Address = pAddress;
            Port = Default.Port;
            Connected = false;

            if (Connected) throw new Exception("Connection to the server is already exist");
            var listenTask = new Task(Listen);
            listenTask.Start();
            return;
        }

        /// <summary>
        /// Listen at the current socket and interpret the received commands.
        /// </summary>
        private static async void Listen()
        {
            await TcpSocketClient.ConnectAsync(Address, Port);

            Connected = true;
            while (Connected)
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

            TcpSocketClient.ReadStream.Read(size, 0, size.Length);

            var length = size.Select((t, i) => (int) (t*Math.Pow(128, i))).Sum();
            var data = new byte[length];
            TcpSocketClient.ReadStream.Read(data, 0, data.Length);

            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// Send command to the current socket.
        /// </summary>
        /// <param name="pCommand">Json command</param>
        public static async void SendCmd(string pCommand)
        {
            if (Connected)
            {
                checkCmd(pCommand);

                var command = Encoding.UTF8.GetBytes(pCommand);
                var size = new byte[4];
                var rest = pCommand.Length;
                for (var i = 0; i < size.Length; i++)
                {
                    size[size.Length - (i + 1)] = (byte) (rest/Math.Pow(128, size.Length - (i + 1)));
                    rest = (int) (rest%Math.Pow(128, size.Length - (i + 1)));
                }
                
                TcpSocketClient.WriteStream.Write(size, 0, size.Length);
                await TcpSocketClient.WriteStream.FlushAsync();

                TcpSocketClient.WriteStream.Write(command, 0, command.Length);
                await TcpSocketClient.WriteStream.FlushAsync();
                return;
            }
            throw new Exception("There is no connection to the server");
        }

        /// <summary>
        /// Disconnect the current device an dispose all ressources
        /// </summary>
        public static async void Disconnect()
        {
            Connected = false;
            await TcpSocketClient.DisconnectAsync();
            TcpSocketClient.Dispose();
        }

        /// <summary>
        /// Close the current connection, for start of the closing.
        /// </summary>
        public static void Close()
        {
            if (!Connected) throw new Exception("There is no connection to the server");
            var command = new Commands.Connection(CommandType.Type.Connection.ToString(), ConnectionType.Type.Disconnect.ToString(), new Commands.Client(Id));
            SendCmd(command.GetCommand());
            return;
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
                if (!int.TryParse(t, out value))
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

        public static string Type => type;
    }
}
