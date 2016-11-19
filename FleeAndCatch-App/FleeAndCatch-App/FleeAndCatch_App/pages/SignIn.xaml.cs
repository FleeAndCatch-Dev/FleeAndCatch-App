using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using FleeAndCatch_App.pages.detail;
using FleeAndCatch_App.pages.master;
using Newtonsoft.Json.Linq;
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

            Application.Current.MainPage = new Home();
        }
        private void BOptions_OnClicked(object sender, EventArgs e)
        {
        }
    }
}
