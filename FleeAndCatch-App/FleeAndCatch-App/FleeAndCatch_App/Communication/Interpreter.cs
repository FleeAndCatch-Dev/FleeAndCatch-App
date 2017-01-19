using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch_App.Controller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch_App.Communication
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
            var id = (CommandType)Enum.Parse(typeof(CommandType), Convert.ToString(jsonCommand.SelectToken("id")));
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
            var type = (SynchronizationType)Enum.Parse(typeof(SynchronizationType), Convert.ToString(pCommand.SelectToken("type")));
            var command = JsonConvert.DeserializeObject<Synchronization>(JsonConvert.SerializeObject(pCommand));
            switch (type)
            {
                case SynchronizationType.All:
                    RobotController.Robots = command.Robots;
                    RobotController.Updated = true;
                    return;
                case SynchronizationType.Current:
                    for (var i = 0; i < RobotController.Robots.Count; i++)
                    {
                        foreach (var t in command.Robots)
                        {
                            if (RobotController.Robots[i].Identification == t.Identification)
                                RobotController.Robots[i] = t;
                        }
                    }
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
