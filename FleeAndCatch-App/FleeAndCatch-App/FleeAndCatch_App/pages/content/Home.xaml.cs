using System;
using Communication;
using Xamarin.Forms;

namespace FleeAndCatch_App.pages.content
{
    public partial class Home : ContentPage
    {

        public Home()
        {
            InitializeComponent();
        }

        private async void BHelp_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Help());
        }

        private async void BLogOut_OnClicked(object sender, EventArgs e)
        {
            if (Client.Connected)
            {
                Client.Close();
                var page = new NavigationPage(new SignIn())
                {
                    BarBackgroundColor = Color.FromHex("#008B8B"),
                    BarTextColor = Color.White
                };
                Application.Current.MainPage = page;
                return;
            }
            await DisplayAlert("Error", "Ups, that should not happen, please start the application again", "OK");
        }
    }
}
