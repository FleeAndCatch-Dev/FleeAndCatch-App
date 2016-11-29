using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Data.Json;
using Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Robots;

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
            var id = (CommandType.Type) Enum.Parse(typeof(CommandType.Type), Convert.ToString(jsonCommand.SelectToken("id")));
            switch (id)
            {
                case CommandType.Type.Connection:
                    Connection(jsonCommand);
                    return;
                case CommandType.Type.Synchronisation:
                    Synchronisation(jsonCommand);
                    return;
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
            var type = (ConnectionType.Type)Enum.Parse(typeof(ConnectionType.Type), Convert.ToString(pCommand.SelectToken("type")));
            var command = JsonConvert.DeserializeObject<Connection>(JsonConvert.SerializeObject(pCommand));
            switch (type)
            {
                case ConnectionType.Type.SetId:
                    Client.Id = command.Identification.Id;
                    return;
                case ConnectionType.Type.GetType:
                    var cmd = new Connection(CommandType.Type.Connection.ToString(), ConnectionType.Type.SetType.ToString(), new Identification(Client.Id, Client.Address, Client.Port, Client.Type, Client.Subtype));
                    Client.SendCmd(cmd.GetCommand());
                    return;
                case ConnectionType.Type.Disconnect:
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
        private static void Synchronisation(JObject pCommand)
        {
            if (pCommand == null) throw new ArgumentNullException(nameof(pCommand));
            var type = (SynchronisationType.Type) Enum.Parse(typeof(SynchronisationType.Type), Convert.ToString(pCommand.SelectToken("type")));
            var command = JsonConvert.DeserializeObject<Synchronisation>(JsonConvert.SerializeObject(pCommand));
            switch (type)
            {
                case SynchronisationType.Type.SetRobots:
                    RobotController.Robots = command.Robots;
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }          
        }
    }
}

