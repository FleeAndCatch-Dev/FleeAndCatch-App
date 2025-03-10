﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models.Szenarios;
using FleeAndCatch.Communication;
using FleeAndCatch.Controller;
using FleeAndCatch_App.Models;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace FleeAndCatch_App.PageModels
{
    public class SzenarioListPageModel : FreshMvvm.FreshBasePageModel
    {
        public List<SzenarioGroupModel> SzenarioGroupList { get; set; }
        private SzenarioGroupModel _szenarioGroup;

        public SzenarioListPageModel()
        {
            SzenarioGroupList = new List<SzenarioGroupModel>();
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            SzenarioGroupList = new List<SzenarioGroupModel>();

            //Get all current szenarios
            RobotController.Updated = false;
            var command = new Synchronization(CommandType.Synchronization.ToString(), SynchronizationCommandType.AllSzenarios.ToString(), Client.Identification, SzenarioController.Szenarios, RobotController.Robots);
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

            SzenarioController.Updated = false;
            var connectionTask = new Task(UpdateSzenarioList);
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
                    var connectionTask = new Task(UpdateSzenarioList);
                    connectionTask.Start();
                });
            }
        }

        public SzenarioGroupModel SelectedItem
        {
            get { return _szenarioGroup; }
            set
            {
                _szenarioGroup = value;
                Szenario szenario = null;
                foreach (var t in SzenarioController.Szenarios)
                {
                    if (t.Id == _szenarioGroup.Number)
                        szenario = t;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                    CoreMethods.PushPageModel<SpectatorPageModel>(szenario);
                    RaisePropertyChanged();
                });

            }
        }

        /// <summary>
        /// Update the user interface and the current robotlist
        /// </summary>
        private async void UpdateSzenarioList()
        {
            if (Client.Connected)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.ShowLoading();
                });

                if (!Client.Connected)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await CoreMethods.DisplayAlert("Error: 323", "No connection to the backend", "OK");
                    });
                    return;
                }

                var counter = 0;
                while (!SzenarioController.Updated && counter <= 300)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(10));
                    counter++;
                }

                //Show exception, when there aren't any szanrios alive
                if (SzenarioController.Szenarios.Count == 0)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        UserDialogs.Instance.HideLoading();
                        await CoreMethods.DisplayAlert("Error", "There aren't any szenarios alive", "OK");
                        await CoreMethods.PopPageModel();
                    });
                    return;
                }                   

                //Update the list with sorting of the szenario types
                SzenarioGroupList.Clear();
                var tempList = new List<SzenarioGroupModel>();
                for (var i = 0; i < Enum.GetNames(typeof(SzenarioCommandType)).Length; i++)
                {
                    foreach (var t in SzenarioController.Szenarios)
                    {
                        if (t.Type == Enum.GetNames(typeof(SzenarioCommandType))[i])
                        {
                            tempList.Add(new SzenarioGroupModel(Enum.GetNames(typeof(SzenarioCommandType))[i], t.Id));
                        }
                    }
                }
                SzenarioGroupList = tempList;

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
