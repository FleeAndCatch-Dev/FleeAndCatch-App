using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models.Szenarios;
using FleeAndCatch_App.Communication;
using FleeAndCatch_App.Controller;
using FleeAndCatch_App.Models;
using Xamarin.Forms;
using Command = Xamarin.Forms.Command;

namespace FleeAndCatch_App.PageModels
{
    public class SzenarioListPageModel : FreshMvvm.FreshBasePageModel
    {
        public List<SzenarioGroup> SzenarioGroupList { get; set; }
        private SzenarioGroup _szenarioGroup;
        //private SzenarioCommandType _szenarioType;

        public SzenarioListPageModel()
        {
            SzenarioGroupList = new List<SzenarioGroup>();
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            //_szenarioType = (SzenarioCommandType)initData;
            SzenarioGroupList = new List<SzenarioGroup>();

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

            UserDialogs.Instance.ShowLoading();
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

        public SzenarioGroup SelectedItem
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
                
                while (!SzenarioController.Updated)
                    await Task.Delay(TimeSpan.FromMilliseconds(10));

                SzenarioGroupList.Clear();
                var tempList = new List<SzenarioGroup>();
                for (var i = 0; i < Enum.GetNames(typeof(SzenarioCommandType)).Length; i++)
                {
                    foreach (var t in SzenarioController.Szenarios)
                    {
                        if (t.Type == Enum.GetNames(typeof(SzenarioCommandType))[i])
                        {
                            tempList.Add(new SzenarioGroup(Enum.GetNames(typeof(SzenarioCommandType))[i], t.Id));
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
