using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Communication;
using FleeAndCatch_App.pages.detail;
using FleeAndCatch_App.sqlite;
using FleeAndCatch_App.sqlite.database;
using Xamarin.Forms;

namespace FleeAndCatch_App.pages
{
    public partial class SignIn : ContentPage
    {
        private Client _client;

        public SignIn()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AiConnect.IsRunning = false;
            AiConnect.IsVisible = false;

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
            AiConnect.IsRunning = false;
            AiConnect.IsVisible = false;

            base.OnDisappearing();
        }

        private async void Connect()
        {
            try
            {
                _client.Connect();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                return;
            }
        }

        private async void BConnect_OnClicked(object sender, EventArgs e)
        {
            try
            {
                _client = new Client(EAddress.Text);               
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
                return;
            }
            var connectionTask = new Task(Connect);
            connectionTask.Start();

            AiConnect.IsRunning = true;
            AiConnect.IsVisible = true;

            while (!_client.Connected)
            {
                //Wait for connection
            }

            if (SSave.IsToggled)
            {
                var connections = SQLiteDB.Connection.GetConnections();
                foreach (var t in connections)
                {
                    t.Save = false;
                    SQLiteDB.Connection.UpdateConnection(t);
                }
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
                SQLiteDB.Connection.AddConnection(EAddress.Text, false);
            }

            Application.Current.MainPage = new Home();
        }
        private void BOptions_OnClicked(object sender, EventArgs e)
        {
        }
    }
}
