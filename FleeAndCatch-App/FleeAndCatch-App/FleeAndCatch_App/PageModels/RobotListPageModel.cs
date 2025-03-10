﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Commands.Models.Szenarios;
using FleeAndCatch.Communication;
using FleeAndCatch.Controller;
using FleeAndCatch_App.Models;
using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;
using Exception = FleeAndCatch.Exception;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class RobotListPageModel : FreshMvvm.FreshBasePageModel
    {
        public List<RobotGroupModel> RobotGroupList { get; set; }
        private SzenarioCommandType _szenarioType;

        public RobotListPageModel()
        {
            RobotGroupList = new List<RobotGroupModel>();
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            _szenarioType = (SzenarioCommandType) initData;
            RobotGroupList = new List<RobotGroupModel>();

            //Send synchronization command to get all robots
            RobotController.Updated = false;
            var command = new Synchronization(CommandType.Synchronization.ToString(), SynchronizationCommandType.AllRobots.ToString(), Client.Identification, SzenarioController.Szenarios, RobotController.Robots);
            Client.SendCmd(command.ToJsonString());
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
                return new Command(async () =>
                {
                    if (RobotGroupList.Count <= 0) return;
                    Szenario szenario = null;
                    var appList = new List<FleeAndCatch.Commands.Models.Devices.Apps.App>();
                    var robotList = new List<Robot>();

                    //Add the robots
                    foreach (var t1 in RobotGroupList)
                    {
                        var value = t1.Choosen;
                        var type = (RobotType)Enum.Parse(typeof(RobotType), t1.Name);
                        while (value > 0)
                        {
                            foreach (var t in RobotController.Robots)
                            {
                                if (t.Identification.Subtype != type.ToString() || t.Active) continue;
                                robotList.Add(t);
                                t.Active = true;
                                value--;
                                break;
                            }
                        }
                    }

                    //Add the apps
                    Client.Device.Active = true;
                    FleeAndCatch.Commands.Models.Devices.Apps.App app = Client.Device as FleeAndCatch.Commands.Models.Devices.Apps.App;
                    appList.Add((FleeAndCatch.Commands.Models.Devices.Apps.App)Client.Device);

                    switch (_szenarioType)
                    {
                        case SzenarioCommandType.Control:
                            if (robotList.Count == 1)
                            {
                                foreach (var t in robotList)
                                {
                                    t.Active = true;
                                    t.Identification.Roletype = RoleType.Undefined.ToString();
                                }
                                Client.Device.Active = true;
                                szenario = new Control(-1, _szenarioType.ToString(), ControlType.Undefinied.ToString(), SzenarioMode.Single.ToString(), appList, robotList, new Steering(0, 0));
                            }
                            break;
                        case SzenarioCommandType.Synchron:
                            if (robotList.Count > 1)
                            {
                                foreach (var t in robotList)
                                {
                                    t.Active = true;
                                    t.Identification.Roletype = RoleType.Undefined.ToString();
                                }
                                Client.Device.Active = true;
                                szenario = new Synchron(-1, _szenarioType.ToString(), ControlType.Undefinied.ToString(), SzenarioMode.Single.ToString(), appList, robotList, new Steering(0, 0));
                            }
                            break;
                        case SzenarioCommandType.Follow:
                            if (robotList.Count > 1)
                            {
                                foreach (var t in robotList)
                                {
                                    t.Active = true;
                                    t.Identification.Roletype = RoleType.Undefined.ToString();
                                }
                                Client.Device.Active = true;
                                szenario = new Follow(-1, _szenarioType.ToString(), ControlType.Undefinied.ToString(), SzenarioMode.Single.ToString(), appList, robotList, new Steering(0, 0));
                            }
                            break;
                        case SzenarioCommandType.Flee:
                            if (robotList.Count > 1)
                            {
                                foreach (var t in robotList)
                                {
                                    t.Active = true;
                                    t.Identification.Roletype = RoleType.Catcher.ToString();
                                }
                                robotList[0].Identification.Roletype = RoleType.Fugitive.ToString();
                                Client.Device.Active = true;
                                szenario = new Flee(-1, _szenarioType.ToString(), ControlType.Undefinied.ToString(), SzenarioMode.Single.ToString(), appList, robotList, new Steering(0, 0));
                            }
                            break;
                        case SzenarioCommandType.Catch:
                            if (robotList.Count > 1)
                            {
                                foreach (var t in robotList)
                                {
                                    t.Active = true;
                                    t.Identification.Roletype = RoleType.Fugitive.ToString();
                                }
                                robotList[0].Identification.Roletype = RoleType.Catcher.ToString();
                                Client.Device.Active = true;
                                szenario = new Catch(-1, _szenarioType.ToString(), ControlType.Undefinied.ToString(), SzenarioMode.Single.ToString(), appList, robotList, new Steering(0, 0));
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    if (szenario != null)
                    {
                        //Set the szenario in the client
                        Client.Szenario = szenario;
                        SzenarioController.Exist = false;

                        var cmd = new SzenarioCommand(CommandType.Szenario.ToString(), SzenarioCommandType.Init.ToString(), Client.Identification, szenario);
                        Client.SendCmd(cmd.ToJsonString());

                        var updateCounter = 0;
                        while (!SzenarioController.Exist && updateCounter <= 300)
                        {
                            await Task.Delay(TimeSpan.FromMilliseconds(10));
                            updateCounter++;
                        }

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            UserDialogs.Instance.HideLoading();
                            if (Client.Szenario.Id != -1)
                            {
                                CoreMethods.PushPageModel<SzenarioInformationPageModel>();
                                RaisePropertyChanged();
                            }
                            else
                            {
                                CoreMethods.DisplayAlert("Error: 3new", "The szenario could not generate", "OK");
                            }                          
                        });
                        return;
                    }
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        UserDialogs.Instance.HideLoading();
                        await CoreMethods.DisplayAlert("Error: 317", "The szenario could not generate", "OK");
                    });
                });
            }
        }

        /// <summary>
        /// Update the user interface and the current robotlist
        /// </summary>
        private async void UpdateRobotList()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.ShowLoading();
                });

                if (!Client.Connected)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await CoreMethods.DisplayAlert("Error: 322", "No connection to the backend", "OK");
                    });
                    return;
                }

                var updateCounter = 0;
                while (!RobotController.Updated && updateCounter <= 300)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(10));
                    updateCounter++;
                }

                if (RobotController.Updated)
                {
                    RobotGroupList.Clear();
                    var tempList = new List<RobotGroupModel>();
                    for (var i = 0; i < Enum.GetNames(typeof(RobotType)).Length; i++)
                    {
                        var counter = 0;
                        foreach (var t in RobotController.Robots)
                        {
                            if (t.Identification.Subtype == Enum.GetNames(typeof(RobotType))[i] && !t.Active)
                                counter++;
                        }
                        tempList.Add(new RobotGroupModel(Enum.GetNames(typeof(RobotType))[i], counter));
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
                    await CoreMethods.DisplayAlert("Error: 318", "The robot could not be update", "OK");
                });
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    UserDialogs.Instance.HideLoading();
                    await CoreMethods.DisplayAlert("Error: " + Convert.ToString(ex.Id), ex.Message, "OK");
                });
                throw;
            }
        }
    }
}
