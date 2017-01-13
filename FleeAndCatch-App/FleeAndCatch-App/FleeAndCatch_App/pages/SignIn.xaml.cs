using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Communication;
using FleeAndCatch_App.pages.content;
using FleeAndCatch_App.sqlite;
using FleeAndCatch_App.sqlite.database;
using Xamarin.Forms;

namespace FleeAndCatch_App.pages
{
    public partial class SignIn : ContentPage
    {
        public SignIn()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var connections = SQLiteDB.Connection.GetConnections();
            foreach (var t in connections)
            {
                if (t.Save != true) continue;
                EAddress.Text = t.Address;
                SSave.IsToggled = true;
                break;
            }
        }
        protected override void OnDisappearing()
        {
            AIConnect.IsRunning = false;
            AIConnect.IsVisible = false;

            base.OnDisappearing();
        }

        /// <summary>
        /// Connect application to the server and returns an error if the connection fails.
        /// </summary>
        private async void Connect()
        {
            try
            {
                Client.Connect(EAddress.Text);

                while (!Client.Connected)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(5));
                }

                var connections = SQLiteDB.Connection.GetConnections();
                foreach (var t in connections)
                {
                    if (t.Address == EAddress.Text) continue;
                    t.Save = false;
                    SQLiteDB.Connection.UpdateConnection(t);
                }

                if (SSave.IsToggled)
                {
                    if (SQLiteDB.Connection.GetConnection(EAddress.Text) != null)
                    {
                        SQLiteDB.Connection.GetConnection(EAddress.Text).Save = true;
                        SQLiteDB.Connection.UpdateConnection(SQLiteDB.Connection.GetConnection(EAddress.Text));
                    }
                    else
                    {
                        SQLiteDB.Connection.AddConnection(EAddress.Text, true);
                    }
                }
                else
                {
                    if (SQLiteDB.Connection.GetConnection(EAddress.Text) == null)
                    {
                        SQLiteDB.Connection.AddConnection(EAddress.Text, false);
                    }
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    var page = new NavigationPage(new Home
                    {
                        Title = "FleeAndCatch"
                    })
                    {
                        BarBackgroundColor = Color.FromHex("#008B8B"),
                        BarTextColor = Color.White
                    };
                    Application.Current.MainPage = page;
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void BConnect_OnClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EAddress.Text))
            {
                var connectionTask = new Task(Connect);
                connectionTask.Start();

                AIConnect.IsRunning = true;
                AIConnect.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Error", "The address for the communication is empty", "OK");
            }
        }
    }
}
