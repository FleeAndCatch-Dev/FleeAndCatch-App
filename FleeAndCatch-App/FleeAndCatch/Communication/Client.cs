using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Devices;
using FleeAndCatch.Commands.Models.Devices.Apps;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Commands.Models.Szenarios;
using Newtonsoft.Json.Linq;
using Sockets.Plugin;

namespace FleeAndCatch.Communication
{
    public static class Client
    {
        //Represents the socket for communication
        private static TcpSocketClient tcpSocketClient;
        //Represents the status of the connection
        private static bool connected;
        //Represents the identification
        private static ClientIdentification identification;
        //Represents the current device
        private static App device;
        //Represents the running szenario
        private static Szenario szenario;

        /// <summary>
        /// Create a new task with a new communication. Set the device and the status.
        /// </summary>
        public static void Connect(string pAddress)
        {
            ParseAddress(pAddress);

            tcpSocketClient = new TcpSocketClient();
            identification = new ClientIdentification(0, IdentificationType.App.ToString(), pAddress, Default.Port);
            Device = new FleeAndCatch.Commands.Models.Devices.Apps.App(new AppIdentification(-1, IdentificationType.App.ToString(), RoleType.Undefined.ToString()));
            connected = false;

            if (connected) throw new Exception(309, "Connection to the server is already exist");
            var listenTask = new Task(Listen);
            listenTask.Start();
        }

        /// <summary>
        /// Listen at the current socket and interpret the received commands.
        /// </summary>
        private static async void Listen()
        {
            try
            {
                await tcpSocketClient.ConnectAsync(identification.Address, identification.Port);
            }
            catch (Exception e)
            {
                throw new Exception(301, "The connection could not established");
            }
            if (tcpSocketClient != null)
            {
                connected = true;

                //Send a connection command to transfer the current device
                var command = new ConnectionCommand(CommandType.Connection.ToString(), ConnectionCommandType.Connect.ToString(), identification, Device);
                SendCmd(command.ToJsonString());

                //Receive the packages and put them to the parser
                while (connected)
                {
                    var data = ReceiveCmd();
                    if(data != null)
                        Interpreter.Parse(data);
                }     
                return;
            }
            throw new Exception(302, "The created socket doesn't exist");
        }

        /// <summary>
        /// Receive a command from the current socket.
        /// </summary>
        /// <returns>String: Json command</returns>
        private static string ReceiveCmd()
        {
            var size = new byte[4];
            byte[] data = null;

            try
            {
                tcpSocketClient.ReadStream.Read(size, 0, size.Length);

                var length = size.Select((t, i) => (int)(t * Math.Pow(128, i))).Sum();
                data = new byte[length];

                tcpSocketClient.ReadStream.Read(data, 0, data.Length);
            }
            catch (Exception e)
            {
                throw new Exception(303, "The json command could not receive");
            }          

            var dataString = Encoding.UTF8.GetString(data, 0, data.Length);

            if (!checkCmd(dataString))
                return null;

            return dataString;
        }

        /// <summary>
        /// Send command to the current socket.
        /// </summary>
        /// <param name="pCommand">Json command</param>
        public static async void SendCmd(string pCommand)
        {
            if (!connected) throw new System.Exception("There is no connection to the server");
            checkCmd(pCommand);

            var command = Encoding.UTF8.GetBytes(pCommand);
            var size = new byte[4];
            var rest = pCommand.Length;
            for (var i = 0; i < size.Length; i++)
            {
                size[size.Length - (i + 1)] = (byte)(rest / Math.Pow(128, size.Length - (i + 1)));
                rest = (int)(rest % Math.Pow(128, size.Length - (i + 1)));
            }

            try
            {
                tcpSocketClient.WriteStream.Write(size, 0, size.Length);
                await tcpSocketClient.WriteStream.FlushAsync();

                tcpSocketClient.WriteStream.Write(command, 0, command.Length);
                await tcpSocketClient.WriteStream.FlushAsync();
            }
            catch (Exception e)
            {
                throw new Exception(304, "The json command could not send");
            }
        }

        /// <summary>
        /// Disconnect the current device an dispose all ressources
        /// </summary>
        public static async void Disconnect()
        {
            connected = false;
            try
            {
                await tcpSocketClient.DisconnectAsync();
                tcpSocketClient.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception(305, "The device could not disconnect");
            }
        }

        /// <summary>
        /// Close the current connection, for start of the closing.
        /// </summary>
        public static void Close()
        {
            if (!Connected) throw new Exception(306, "There is no connection to the server");
            //Send connection command to close the connection
            var command = new ConnectionCommand(CommandType.Connection.ToString(), ConnectionCommandType.Disconnect.ToString(), identification, Device);
            SendCmd(command.ToJsonString());
        }

        /// <summary>
        /// Parse the current address of an ip address.
        /// </summary>
        /// <param name="pAddress">Connection address to the server.</param>
        private static void ParseAddress(string pAddress)
        {
            var adressPart = pAddress.Split(new string[] { "." }, StringSplitOptions.None);
            if (adressPart.Length != 4) throw new Exception(307, "The Address could not parse into an ip adress");
            var result = true;
            foreach (var t in adressPart)
            {
                int value;
                if (!int.TryParse(t, out value) && value >= 0 && value <= 255)
                    result = false;
            }
            if (result)
                return;
            throw new Exception(307, "The Address could not parse into an ip adress");
        }

        /// <summary>
        /// Check the current command of a json command.
        /// </summary>
        /// <param name="pCommand">Json command</param>
        private static bool checkCmd(string pCommand)
        {
            try
            {
                var data = JObject.Parse(pCommand);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public static bool Connected => connected;
        public static ClientIdentification Identification => identification;
        public static App Device
        {
            get { return device; }
            set { device = value; }
        }

        public static Szenario Szenario
        {
            get { return szenario; }
            set { szenario = value; }
        }
    }

    public static class Default
    {
        public static int Port = 5000;
        public static int TimeOut = 300;
    }
}
