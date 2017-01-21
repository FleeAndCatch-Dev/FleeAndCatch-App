using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.Content.Res;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Commands.Models.Szenarios;
using FleeAndCatch.Components;
using FleeAndCatch_App.Communication;
using FleeAndCatch_App.Controller;
using FleeAndCatch_App.Models;
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

        public override void Init(object initData)
        {
            base.Init(initData);

            _szenarioType = (SzenarioCommandType) initData;
            RobotGroupList = new List<RobotGroup>();
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            UserDialogs.Instance.ShowLoading();
            var connectionTask = new Task(UpdateRobotList);
            connectionTask.Start();
        }

        public RobotGroup RobotList_OnItemSelected
        {
            get { return null; }
            set
            {
                if (value.Number > 0)
                {
                    Robot robot = null;
                    foreach (var t in RobotController.Robots)
                    {
                        if (value.Name == t.Identification.Subtype)
                            robot = t;
                    }

                    SzenarioCommand cmd = null;
                    Szenario szenario = null;
                    var appList = new List<FleeAndCatch.Commands.Models.Devices.Apps.App>();
                    var robotList = new List<Robot>();
                    switch (_szenarioType)
                    {
                        case SzenarioCommandType.Control:
                            if (robot != null)
                            {
                                robot.Active = true;
                                appList.Add((FleeAndCatch.Commands.Models.Devices.Apps.App) Client.Device);
                                robotList.Add(robot);
                                szenario = new Control(_szenarioType.ToString(), ControlType.Begin.ToString(), appList, robotList, new Steering(0, 0));
                            }
                            break;
                        case SzenarioCommandType.Synchron:
                            break;
                        case SzenarioCommandType.Follow:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    cmd = new SzenarioCommand(CommandType.Szenario.ToString(), _szenarioType.ToString(), Client.Identification, szenario);
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
                    await CoreMethods.DisplayAlert("Error", "There isn't a robot of that group", "OK");
                });
            }
        }

        private async void UpdateRobotList()
        {
            if (Client.Connected)
            {
                RobotController.Updated = false;
                var command = new Synchronization(CommandType.Synchronization.ToString(), SynchronizationCommandType.All.ToString(), Client.Identification, RobotController.Robots);
                Client.SendCmd(command.GetCommand());

                Device.BeginInvokeOnMainThread( () =>
                {
                    UserDialogs.Instance.ShowLoading();
                });

                while (!RobotController.Updated)
                    await Task.Delay(TimeSpan.FromMilliseconds(10));

                RobotGroupList = new List<RobotGroup>();

                for (int i = 0; i < Enum.GetNames(typeof(ComponentType.RobotType)).Length; i++)
                {
                    var counter = 0;
                    foreach (var t in RobotController.Robots)
                    {
                        if (t.Identification.Subtype == Enum.GetNames(typeof(ComponentType.RobotType))[i] && !t.Active)
                            counter++;
                    }
                    RobotGroupList.Insert(RobotGroupList.Count, new RobotGroup { Name = Enum.GetNames(typeof(ComponentType.RobotType))[i], Number = counter });
                }
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
