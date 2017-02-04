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
                    Szenario(jsonCommand);
                    return;
                case CommandType.Exception:
                    Exception(jsonCommand);
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
            var command = JsonConvert.DeserializeObject<Synchronization>(JsonConvert.SerializeObject(pCommand));
            var type = (SynchronizationCommandType)Enum.Parse(typeof(SynchronizationCommandType), command.Type);

            switch (type)
            {
                case SynchronizationCommandType.All:
                    RobotController.Robots = command.Robots;
                    RobotController.Updated = true;
                    return;
                case SynchronizationCommandType.Current:
                    for (var i = 0; i < RobotController.Robots.Count; i++)
                    {
                        if (RobotController.Robots[i].Identification.Id != Client.Identification.Id) continue;
                        //Update robot object instances
                        RobotController.Robots[i] = command.Robots[0];
                        Client.Device = command.Robots[0];
                        SzenarioController.ChangedPosition = true;
                        break;
                    }
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Parse a szenario command.
        /// </summary>
        /// <param name="pCommand"></param>
        private static void Szenario(JObject jsonCommand)
        {
            //Handle szenario command TODO
        }

        /// <summary>
        /// Parse a exception command.
        /// </summary>
        /// <param name="pCommand"></param>
        private static void Exception(JObject pCommand)
        {
            if (pCommand == null) throw new ArgumentNullException(nameof(pCommand));
            var command = JsonConvert.DeserializeObject<ExceptionCommand>(JsonConvert.SerializeObject(pCommand));
            var type = (ExceptionCommandType)Enum.Parse(typeof(ExceptionCommandType), command.Type);        

            switch (type)
            {
                case ExceptionCommandType.Undefined:
                    throw new Java.Lang.Exception(command.Exception.Message);
                case ExceptionCommandType.UnhandeldDisconnection:
                    //Other device is disconnecting 
                    var app = Client.Device as FleeAndCatch.Commands.Models.Devices.Apps.App;
                    if (app != null && app.Active)
                    {
                        //Handle UnhandeldDisconnection
                       SzenarioController.Refresh = false;
                        //App navigates automatic to home page
                    }
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
