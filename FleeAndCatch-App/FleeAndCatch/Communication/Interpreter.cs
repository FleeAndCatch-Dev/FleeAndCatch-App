using System;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models.Szenarios;
using FleeAndCatch.Controller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FleeAndCatch.Communication
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
                throw new System.Exception("Wrong apiid in json command");
            var id = (CommandType) Enum.Parse(typeof(CommandType), Convert.ToString(jsonCommand.SelectToken("id")));
            switch (id)
            {
                case CommandType.Connection:
                    Connection(jsonCommand);
                    return;
                case CommandType.Synchronization:
                    Synchronization(jsonCommand);
                    return;
                case CommandType.Szenario:
                    Szenario(jsonCommand);
                    return;
                case CommandType.Exception:
                    Exception(jsonCommand);
                    return;
                default:
                    throw new Exception(310, "Wrong command type of json command");
            }
        }

        /// <summary>
        /// Parse a connection command.
        /// </summary>
        /// <param name="pCommand"></param>
        private static void Connection(JObject pCommand)
        {
            if (pCommand == null) throw new Exception(311, "There doesn't exist a json command");
            var command = JsonConvert.DeserializeObject<ConnectionCommand>(JsonConvert.SerializeObject(pCommand));
            var type = (ConnectionCommandType) Enum.Parse(typeof(ConnectionCommandType), command.Type);
            

            switch (type)
            {
                case ConnectionCommandType.Connect:
                    //Set the id to the objects
                    Client.Identification.Id = command.Identification.Id;
                    var app = (FleeAndCatch.Commands.Models.Devices.Apps.App) Client.Device;
                    app.Identification.Id = command.Identification.Id;
                    return;
                case ConnectionCommandType.Disconnect:
                    Client.Disconnect();
                    return;
                case ConnectionCommandType.Init:
                    //Send a connection command for initialization
                    var cmd = new ConnectionCommand(CommandType.Connection.ToString(), ConnectionCommandType.Init.ToString(), Client.Identification, Client.Device);
                    Client.SendCmd(cmd.ToJsonString());
                    return;
                default:
                    throw new Exception(312, "Wrong connection type of json command");
            }
        }

        /// <summary>
        /// Parse a synchronisation command.
        /// </summary>
        /// <param name="pCommand"></param>
        private static void Synchronization(JObject pCommand)
        {
            if (pCommand == null) throw new Exception(311, "There doesn't exist a json command");
            var command = JsonConvert.DeserializeObject<Synchronization>(JsonConvert.SerializeObject(pCommand));
            var type = (SynchronizationCommandType)Enum.Parse(typeof(SynchronizationCommandType), command.Type);

            switch (type)
            {
                case SynchronizationCommandType.AllRobots:
                    RobotController.Robots = command.Robots;
                    RobotController.Updated = true;
                    return;
                case SynchronizationCommandType.CurrentRobot:
                    for (var i = 0; i < RobotController.Robots.Count; i++)
                    {
                        foreach (var t in command.Robots)
                        {
                            if (RobotController.Robots[i].Identification.Id != t.Identification.Id) continue;
                            //Update robot object instances
                            RobotController.Robots[i] = t;
                            SzenarioController.Changed = true;
                            break;
                        }
                    }
                    return;
                case SynchronizationCommandType.AllSzenarios:
                    SzenarioController.Szenarios = command.Szenarios;
                    SzenarioController.Updated = true;
                    break;
                case SynchronizationCommandType.CurrentSzenario:
                    for (var i = 0; i < SzenarioController.Szenarios.Count; i++)
                    {
                        foreach (var t in command.Szenarios)
                        {
                            if (SzenarioController.Szenarios[i].Id != t.Id) continue;
                            //Update szenario
                            SzenarioController.Szenarios[i] = t;
                            SzenarioController.Changed = true;
                        }
                    }
                    break;
                default:
                    throw new Exception(313, "Wrong synchronization type of json command");
            }
        }

        private static void Szenario(JObject pCommand)
        {
            if (pCommand == null) throw new Exception(311, "There doesn't exist a json command");
            var command = JsonConvert.DeserializeObject<SzenarioCommand>(JsonConvert.SerializeObject(pCommand));
            var type = (SzenarioCommandType)Enum.Parse(typeof(SzenarioCommandType), command.Type);

            switch (type)
            {
                case SzenarioCommandType.Init:
                    //Set the id of the szenario
                    Client.Szenario = command.Szenario;
                    SzenarioController.Exist = true;
                    return;
                case SzenarioCommandType.End:
                    //Set the szenario to false and return to home page
                    SzenarioController.Refresh = false;
                    return;
                case SzenarioCommandType.Control:
                    throw new Exception(314, "Wrong szenario type of json command");
                case SzenarioCommandType.Synchron:
                    throw new Exception(314, "Wrong szenario type of json command");
                case SzenarioCommandType.Follow:
                    throw new Exception(314, "Wrong szenario type of json command");
                default:
                    throw new Exception(314, "Wrong szenario type of json command");
            }
        }

        /// <summary>
        /// Parse a exception command.
        /// </summary>
        /// <param name="pCommand"></param>
        private static void Exception(JObject pCommand)
        {
            //Need to test
            if (pCommand == null) throw new Exception(311, "There doesn't exist a json command");
            var command = JsonConvert.DeserializeObject<ExceptionCommand>(JsonConvert.SerializeObject(pCommand));
            var type = (ExceptionCommandType)Enum.Parse(typeof(ExceptionCommandType), command.Type);        

            switch (type)
            {
                case ExceptionCommandType.Undefined:
                    throw new System.Exception(command.Exception.Message);
                case ExceptionCommandType.UnhandeldDisconnection:
                    //Other device is disconnecting 
                    var app = (FleeAndCatch.Commands.Models.Devices.Apps.App)Client.Device;
                    if (app != null && app.Active)
                    {
                        //Handle UnhandeldDisconnection
                       SzenarioController.Refresh = false;
                        //App navigates automatic to home page
                    }
                    return;
                case ExceptionCommandType.CreateSzenario:
                    SzenarioController.Exist = true;
                    //throw exception in user interface -> RobotListPageModel
                    break;
                default:
                    throw new Exception(315, "Wrong exception type of json command");
            }
        }
    }
}
