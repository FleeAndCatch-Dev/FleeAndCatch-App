using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Components;
using FleeAndCatch_App.Controller;
using FleeAndCatch_App.PageModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

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
                throw new Java.Lang.Exception("Wrong apiid in json command");
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
                    throw new ArgumentOutOfRangeException();
                case CommandType.Exception:
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
            var type = (ConnectionCommandType) Enum.Parse(typeof(ConnectionCommandType), Convert.ToString(pCommand.SelectToken("type")));
            var command = JsonConvert.DeserializeObject<ConnectionCommand>(JsonConvert.SerializeObject(pCommand));

            switch (type)
            {
                case ConnectionCommandType.Connect:
                    Client.Identification.Id = command.Identification.Id;
                    var app = (FleeAndCatch.Commands.Models.Devices.Apps.App) Client.Device;
                    app.Identification.Id = command.Identification.Id;
                    return;
                case ConnectionCommandType.Disconnect:
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
            var type = (SynchronizationCommandType)Enum.Parse(typeof(SynchronizationCommandType), Convert.ToString(pCommand.SelectToken("type")));
            var command = JsonConvert.DeserializeObject<Synchronization>(JsonConvert.SerializeObject(pCommand));
            switch (type)
            {
                case SynchronizationCommandType.All:
                    RobotController.Robots = command.Robots;
                    RobotController.Updated = true;
                    return;
                case SynchronizationCommandType.Current:
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
