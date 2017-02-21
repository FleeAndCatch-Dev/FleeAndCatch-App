using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FleeAndCatch.Communication;
using FleeAndCatch_App.Models;
using FleeAndCatch_App.SQLite;
using Xamarin.Forms;
using PropertyChanged;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class SignInPageModel : FreshMvvm.FreshBasePageModel
    {
        public ConnectionModel Connection { get; set; }
        public bool Visible { get; set; }

        public override void Init(object initData)
        {
            base.Init(initData);

            Connection = new ConnectionModel();
        }

        /// <summary>
        /// Get a saved address from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            var connections = SQLiteDB.Connection.GetConnections();
            foreach (var t in connections)
            {
                if (t.Save != true) continue;
                Connection.Address = t.Address;
                Connection.Save = t.Save;
                break;
            }
        }

        public Command BSignIn_OnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!string.IsNullOrEmpty(Connection.Address))
                    {
                        UserDialogs.Instance.ShowLoading();
                        var connectionTask = new Task(Connect);
                        connectionTask.Start();
                    }
                    else
                        await CoreMethods.DisplayAlert("Error", "The address for the communication is empty", "OK");
                });
            }
        }

        /// <summary>
        /// Connect application to the server and returns an error if the connection fails.
        /// </summary>
        private async void Connect()
        {
            try
            {
                Client.Connect(Connection.Address);

                for (var i = 0; i < Default.TimeOut; i++)
                {
                    if(Client.Connected)
                        break;
                    await Task.Delay(TimeSpan.FromMilliseconds(10));
                }

                if (Client.Connected)
                {
                    var connections = SQLiteDB.Connection.GetConnections();
                    foreach (var t in connections)
                    {
                        t.Save = false;
                        SQLiteDB.Connection.UpdateConnection(t);
                    }

                    if (Connection.Save)
                    {
                        if (SQLiteDB.Connection.GetConnection(Connection.Address) != null)
                        {
                            var localConnection = SQLiteDB.Connection.GetConnection(Connection.Address);
                            localConnection.Save = true;
                            SQLiteDB.Connection.UpdateConnection(localConnection);
                        }
                        else
                            SQLiteDB.Connection.AddConnection(Connection.Address, true);
                    }

                    Device.BeginInvokeOnMainThread(() => {
                        UserDialogs.Instance.HideLoading();
                        var page = FreshMvvm.FreshPageModelResolver.ResolvePageModel<HomePageModel>();
                        var navigation = new FreshMvvm.FreshNavigationContainer(page)
                        {
                            BarBackgroundColor = Color.FromHex("#008B8B"),
                            BarTextColor = Color.White
                        };
                        Application.Current.MainPage = navigation;
                    });
                    return;
                }
                Device.BeginInvokeOnMainThread(async () => {
                    UserDialogs.Instance.HideLoading();
                    await CoreMethods.DisplayAlert("Error", "Connection timeout, try again", "OK");
                });
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    UserDialogs.Instance.HideLoading();
                    await CoreMethods.DisplayAlert("Error", ex.Message, "OK");
                });
            }
        }
    }
}
