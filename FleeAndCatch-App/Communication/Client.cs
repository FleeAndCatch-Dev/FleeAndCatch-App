using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sockets.Plugin;

namespace Communication
{
    public class Client
    {
        #region variables

        private readonly TcpSocketClient _client;
        private readonly string _address;
        private readonly int _port;
        private bool _connected;

        private Task _listenTask;

        #endregion

        #region methods

        /// <summary>
        /// Create a client with the default settings.
        /// </summary>
        public Client()
        {
            _client = new TcpSocketClient();
            _address = Default.Address;
            _port = Default.Port;
        }

        /// <summary>
        /// Create a client with the given parameters.
        /// </summary>
        /// <param name="pAddress"></param>
        /// <param name="pPort"></param>
        public Client(string pAddress, int pPort)
        {
            _client = new TcpSocketClient();
            _address = pAddress;
            _port = pPort;
        }

        /// <summary>
        /// Connect to the server and create a new thread to run there a listener.
        /// </summary>
        public async void Connect()
        {
            if (!_connected)
            {
                try
                {
                    await _client.ConnectAsync(_address, _port);
                    _connected = true;

                    _listenTask = new Task(Listen);
                    _listenTask.Start();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Disconnect from the server and close the current listener thread.
        /// </summary>
        public async void Disconnect()
        {
            if (!_connected)
            {
                try
                {
                    await _client.DisconnectAsync();
                    _connected = false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Listen on the current address and receive data.
        /// </summary>
        private void Listen()
        {
            var size = new byte[4];

            while (_connected)
            {
                try
                {
                    _client.ReadStream.Read(size, 0, size.Length);

                    var length = 0;
                    for (var i = 0; i < size.Length; i++)
                    {
                        length += (int) (size[size.Length - (i + 1)] * Math.Pow(256, i));
                    }

                    var data = new byte[length];
                    _client.ReadStream.Read(data, 0, data.Length);

                    var test = Encoding.UTF8.GetString(data, 0, data.Length);
                    //New Data
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Sends a json command to the server. First sends an integer as the length of the string and final it sends the data.
        /// </summary>
        /// <param name="pCommand"></param>
        public async void SendCommand(string pCommand)
        {
            var data = Encoding.UTF8.GetBytes(pCommand);
            var sizebytes = BitConverter.GetBytes(data.Length);

            try
            {
                _client.WriteStream.Write(sizebytes, 0, sizebytes.Length);
                await _client.WriteStream.FlushAsync();

                _client.WriteStream.Write(data, 0, data.Length);
                await _client.WriteStream.FlushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Get the current connection of this client.
        /// </summary>
        public bool GetConnected => _connected;

        #endregion
    }
}
