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
        /// Interpret a command and returns a result as string.
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void Connection(JObject pCommand)
        {
            if (pCommand == null) throw new ArgumentNullException(nameof(pCommand));
            var type = (ConnectionType.Type)Enum.Parse(typeof(ConnectionType.Type), Convert.ToString(pCommand.SelectToken("type")));
            var command = JsonConvert.DeserializeObject<Connection>(JsonConvert.SerializeObject(pCommand));
            switch (type)
            {
                case ConnectionType.Type.SetId:
                    Client.Id = command.Client.Id;
                    return;
                case ConnectionType.Type.GetType:
                    var cmd = new Commands.Connection(CommandType.Type.Connection.ToString(), ConnectionType.Type.SetType.ToString(), new Commands.Client(Client.Id));
                    Client.SendCmd(cmd.GetCommand());
                    return;
                case ConnectionType.Type.Disconnect:
                    Client.Disconnect();
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void Home(JObject pCommand)
        {
            /*var type = Convert.ToString(pCommand.SelectToken("type")).ToLower();
            if (type == "setrobots")
            {
                var jsonrobots = pCommand.SelectToken("robots") as JArray;
                if (jsonrobots == null) return;

                //Delete not exist robots
                for (var i = 0; i < RobotController.Robots.Count; i++)
                {
                    var exist = false;
                    for (var j = 0; j < jsonrobots.Count; j++)
                    {
                        var id = Convert.ToInt32(Convert.ToString(jsonrobots[j].SelectToken("id")));
                        if (id == RobotController.Robots[i].Id)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                        RobotController.Robots.Remove(RobotController.Robots[i]);
                }

                //Add new robots
                for (var j = 0; j < jsonrobots.Count; j++)
                {
                    var id = Convert.ToInt32(Convert.ToString(jsonrobots[j].SelectToken("id")));
                    var exist = false;
                    for (var i = 0; i < RobotController.Robots.Count; i++)
                    {
                        if (id == RobotController.Robots[i].Id)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        type = Convert.ToString(jsonrobots[j].SelectToken("type"));
                        switch (type)
                        {
                            case "ThreeWheelRobot":
                                RobotController.Robots.Add(new ThreeWheelDrive(id));
                                break;
                            default:
                                break;
                        }
                    }
                }
                return;
            }
            throw new Exception("Receiving of robots going wrong");*/
        }
    }
}

