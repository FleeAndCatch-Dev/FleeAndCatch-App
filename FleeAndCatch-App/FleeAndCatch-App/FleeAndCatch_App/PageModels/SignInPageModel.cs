using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using FleeAndCatch_App.Models;
using FleeAndCatch_App.SQLite;
using Xamarin.Forms;
using FleeAndCatch_App.Communication;
using PropertyChanged;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class SignInPageModel : FreshMvvm.FreshBasePageModel
    {
        public Connection Connection { get; set; }
        public bool Visible { get; set; }

        public SignInPageModel()
        {
            
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            Connection = new Connection();
        }

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

                while (!Client.Connected)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(5));
                }

                var connections = SQLiteDB.Connection.GetConnections();
                foreach (var t in connections)
                {
                    if (t.Address == Connection.Address) continue;
                    t.Save = false;
                    SQLiteDB.Connection.UpdateConnection(t);
                }

                if (Connection.Save)
                {
                    if (SQLiteDB.Connection.GetConnection(Connection.Address) != null)
                    {
                        SQLiteDB.Connection.GetConnection(Connection.Address).Save = true;
                        SQLiteDB.Connection.UpdateConnection(SQLiteDB.Connection.GetConnection(Connection.Address));
                    }
                    else
                    {
                        SQLiteDB.Connection.AddConnection(Connection.Address, true);
                    }
                }
                else
                {
                    if (SQLiteDB.Connection.GetConnection(Connection.Address) == null)
                    {
                        SQLiteDB.Connection.AddConnection(Connection.Address, false);
                    }
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
