using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Sockets.Plugin;

namespace Communication
{
    public class Client
    {
        private TcpSocketClient _tcpSocketClient;
        private string _address;
        private int _port;
        private int _id;
        private bool _connected;
        private Interpreter _interpreter;

        /// <summary>
        /// Create an object of the class client and a new socket for the communication.
        /// </summary>
        /// <param name="pAddress">Connection address to the server.</param>
        public Client(string pAddress)
        {
            ParseAddress(pAddress);

            this._tcpSocketClient = new TcpSocketClient();
            this._address = pAddress;
            this._port = Default.Port;
            this._connected = false;
        }

        /// <summary>
        /// Create a new task with a new communication.
        /// </summary>
        public void Connect()
        {
            if (_connected) throw new Exception("Connection to the server is already exist");
            var _listenTask = new Task(Listen);
            _listenTask.Start();
            return;
        }

        /// <summary>
        /// Listen at the current socket and interpret the received commands.
        /// </summary>
        private async void Listen()
        {
            await _tcpSocketClient.ConnectAsync(_address, _port);

            _interpreter = new Interpreter(this);
            _id = Convert.ToInt32(_interpreter.Interpret(ReceiveCmd()));

            _connected = true;

            while (_connected)
            {
                var command = ReceiveCmd();
                _interpreter.Interpret(command);
            }

            //await  _tcpSocketClient.DisconnectAsync();
            //_tcpSocketClient.Dispose();
        }

        /// <summary>
        /// Receive a command from the current socket.
        /// </summary>
        /// <returns>String: Json command</returns>
        private string ReceiveCmd()
        {
            var size = new byte[4];

            _tcpSocketClient.ReadStream.Read(size, 0, size.Length);

            var length = size.Select((t, i) => (int) (t*Math.Pow(128, i))).Sum();
            var data = new byte[length];
            _tcpSocketClient.ReadStream.Read(data, 0, data.Length);

            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// Send command to the current socket.
        /// </summary>
        /// <param name="pCommand">Json command</param>
        public async void SendCmd(string pCommand)
        {
            if (_connected)
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
                
                _tcpSocketClient.WriteStream.Write(size, 0, size.Length);
                await _tcpSocketClient.WriteStream.FlushAsync();

                _tcpSocketClient.WriteStream.Write(command, 0, command.Length);
                await _tcpSocketClient.WriteStream.FlushAsync();
                return;
            }
            throw new Exception("There is no connection to the server");
        }

        public async void Disconnect()
        {
            _connected = false;
            await _tcpSocketClient.DisconnectAsync();
            _tcpSocketClient.Dispose();
        }

        /// <summary>
        /// Close the current connection.
        /// </summary>
        public void Close()
        {
            if (_connected)
            {
                SendCmd("{\"id\":\"Client\",\"type\":\"Disconnect\",\"apiid\":\"@@fleeandcatch@@\",\"errorhandling\":\"ignoreerrors\",\"client\":{\"id\":" + _id + ",\"type\":\"App\"}}");
                return;
            }
            throw new Exception("There is no connection to the server");
        }

        /// <summary>
        /// Parse the current address of an ip address.
        /// </summary>
        /// <param name="pAddress">Connection address to the server.</param>
        private void ParseAddress(string pAddress)
        {
            var adressPart = pAddress.Split(new string[] {"."}, StringSplitOptions.None);
            if (adressPart.Length == 4)
            {
                var result = true;
                foreach (var t in adressPart)
                {
                    int value;
                    if (!int.TryParse(t, out value))
                        result = false;
                }
                if (result)
                    return;
            }
            throw new Exception("The Address could not parse into an ip adress");
        }

        /// <summary>
        /// Check the current command of a json command.
        /// </summary>
        /// <param name="pCommand">Json command</param>
        private void checkCmd(string pCommand)
        {
            try
            {
                var jsonObject = new JsonObject();
                jsonObject = JsonObject.Parse(pCommand);
                return;
            }
            catch (Exception)
            {
                throw new Exception("The command could not parse into json");
            }
        }

        public bool Connected => _connected;
        public int Id => _id;
    }
}
