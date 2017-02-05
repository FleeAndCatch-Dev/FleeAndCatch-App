using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Commands.Models.Szenarios;
using FleeAndCatch.Components;
using FleeAndCatch_App.Communication;
using FleeAndCatch_App.Controller;
using FleeAndCatch_App.Models;
using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class RobotListPageModel : FreshMvvm.FreshBasePageModel
    {
        public List<RobotGroup> RobotGroupList { get; set; }
        private SzenarioCommandType _szenarioType;

        public RobotListPageModel()
        {
            RobotGroupList = new List<RobotGroup>();
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            _szenarioType = (SzenarioCommandType) initData;
            RobotGroupList = new List<RobotGroup>();

            RobotController.Updated = false;
            var command = new Synchronization(CommandType.Synchronization.ToString(), SynchronizationCommandType.All.ToString(), Client.Identification, RobotController.Robots);
            Client.SendCmd(command.GetCommand());
        }

        /// <summary>
        /// Start a new task for updating the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            UserDialogs.Instance.ShowLoading();
            var connectionTask = new Task(UpdateRobotList);
            connectionTask.Start();
        }

        /// <summary>
        /// Refresh the listview
        /// </summary>
        public Command Refresh_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    UserDialogs.Instance.ShowLoading();
                    var connectionTask = new Task(UpdateRobotList);
                    connectionTask.Start();
                });
            }
        }

        /// <summary>
        /// Get the selected robots and navigates the user to the szenario page
        /// </summary>
        public Command BContinue_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (RobotGroupList.Count <= 0) return;
                    Szenario szenario = null;
                    var appList = new List<FleeAndCatch.Commands.Models.Devices.Apps.App>();
                    var robotList = new List<Robot>();

                    //Add the robots
                    foreach (var t1 in RobotGroupList)
                    {
                        var value = t1.Choosen;
                        var type = (ComponentType.RobotType) Enum.Parse(typeof(ComponentType.RobotType), t1.Name);
                        while (value > 0)
                        {
                            foreach (var t in RobotController.Robots)
                            {
                                if (t.Identification.Subtype != type.ToString()) continue;
                                t.Active = true;
                                robotList.Add(t);
                                value--;
                                break;
                            }
                        }
                    }

                    //Add the apps
                    Client.Device.Active = true;
                    appList.Add((FleeAndCatch.Commands.Models.Devices.Apps.App)Client.Device);

                    switch (_szenarioType)
                    {
                        case SzenarioCommandType.Control:
                            if (robotList.Count == 1)
                            {
                                foreach (var t in robotList)
                                {
                                    t.Active = true;
                                }
                                Client.Device.Active = true;
                                szenario = new Control(_szenarioType.ToString(), ControlType.Begin.ToString(), SzenarioMode.Single.ToString(), appList, robotList, new Steering(0, 0));
                            }
                            break;
                        case SzenarioCommandType.Synchron:
                            break;
                        case SzenarioCommandType.Follow:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    //Set the szenario in the client
                    Client.Szenario = szenario;
                    if (szenario != null)
                    {
                        var cmd = new SzenarioCommand(CommandType.Szenario.ToString(), _szenarioType.ToString(), Client.Identification, szenario);
                        Client.SendCmd(cmd.GetCommand());

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            UserDialogs.Instance.HideLoading();
                            CoreMethods.PushPageModel<SzenarioInformationPageModel>(szenario);
                            RaisePropertyChanged();
                        });
                        return;
                    }
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        UserDialogs.Instance.HideLoading();
                        await CoreMethods.DisplayAlert("Error", "This inputs aren't correct", "OK");
                    });
                });
            }
        }

        /// <summary>
        /// Update the user interface and the current robotlist
        /// </summary>
        private async void UpdateRobotList()
        {
            if (Client.Connected)
            {
                Device.BeginInvokeOnMainThread( () =>
                {
                    UserDialogs.Instance.ShowLoading();
                });

                while (!RobotController.Updated)
                    await Task.Delay(TimeSpan.FromMilliseconds(10));

                RobotGroupList.Clear();
                var tempList = new List<RobotGroup>();
                for (var i = 0; i < Enum.GetNames(typeof(ComponentType.RobotType)).Length; i++)
                {
                    var counter = 0;
                    foreach (var t in RobotController.Robots)
                    {
                        if (t.Identification.Subtype == Enum.GetNames(typeof(ComponentType.RobotType))[i] && !t.Active)
                            counter++;
                    }
                    tempList.Add(new RobotGroup(Enum.GetNames(typeof(ComponentType.RobotType))[i], counter));
                }
                RobotGroupList = tempList;
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                });
                return;
            }
            Device.BeginInvokeOnMainThread(async () =>
            {
                UserDialogs.Instance.HideLoading();
                await CoreMethods.DisplayAlert("Error", "Ups, there is something wrong, please start the application again", "OK");
            });
        }
    }
}
