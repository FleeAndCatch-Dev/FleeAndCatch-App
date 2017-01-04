using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Data.Json;
using Commands;
using Commands.Devices;
using Commands.Devices.Robots;
using Commands.Identifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Controller;

namespace Communication
{
    public static class Interpreter
    {
        /// <summary>
        /// Parse a json command and runs it an the system, if the parsing is correct.
        /// </summary>
        /// <param name="pCommand">Json command</param>
        public static void Parse(string pCommand)
        {
            var jsonCommand = JObject.Parse(pCommand);
            if (Convert.ToString(jsonCommand.SelectToken("apiid")) != "@@fleeandcatch@@")
                throw new Exception("Wrong apiid in json command");
            var id = (CommandType) Enum.Parse(typeof(CommandType), Convert.ToString(jsonCommand.SelectToken("id")));
            switch (id)
            {
                case CommandType.Connection:
                    Connection(jsonCommand);
                    return;
                case CommandType.Synchronization:
                    Synchronization(jsonCommand);
                    return;
                case CommandType.Control:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Parse a connection command.
        /// </summary>
        /// <param name="pCommand"></param>
        private static void Connection(JObject pCommand)
        {
            if (pCommand == null) throw new ArgumentNullException(nameof(pCommand));
            var type = (ConnectionType)Enum.Parse(typeof(ConnectionType), Convert.ToString(pCommand.SelectToken("type")));


            var command = JsonConvert.DeserializeObject<Connection>(JsonConvert.SerializeObject(pCommand));

            //var command = JsonConvert.DeserializeObject<Connection>(JsonConvert.SerializeObject(pCommand), new IdentificationJsonConverter(typeof(Identification)));

            //Employee newEmployee = JsonConvert.DeserializeObject<Employee>(json, new KeysJsonConverter(typeof(Employee)));

            switch (type)
            {
                case ConnectionType.Connect:
                    Client.Identification.Id = command.Identification.Id;
                    return;
                case ConnectionType.Disconnect:
                    Client.Disconnect();
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Parse a synchronisation command.
        /// </summary>
        /// <param name="pCommand"></param>
        private static void Synchronization(JObject pCommand)
        {
            if (pCommand == null) throw new ArgumentNullException(nameof(pCommand));
            var type = (SynchronizationType) Enum.Parse(typeof(SynchronizationType), Convert.ToString(pCommand.SelectToken("type")));
            var command = JsonConvert.DeserializeObject<Synchronization>(JsonConvert.SerializeObject(pCommand));
            switch (type)
            {
                case SynchronizationType.Robots:
                    RobotController.Robots = command.Robots;
                    RobotController.Updated = true;
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }          
        }
    }
}

