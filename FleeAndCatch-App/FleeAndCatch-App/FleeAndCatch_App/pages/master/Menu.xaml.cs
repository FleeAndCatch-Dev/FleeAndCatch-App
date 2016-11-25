using System;
using Communication;
using FleeAndCatch_App.pages.detail;
using Xamarin.Forms;

namespace FleeAndCatch_App.pages.master
{
    public partial class Menu : MasterDetailPage
    {
        public Menu()
        {
            InitializeComponent();

            var page = new NavigationPage(new Home())
            {
                BarBackgroundColor = Color.FromHex("#008B8B"),
                BarTextColor = Color.White
            };
            Detail = page;
        }

        private void BLogOut_OnClicked(object sender, EventArgs e)
        {
            Client.Close();
            var page = new NavigationPage(new SignIn())
            {
                BarBackgroundColor = Color.FromHex("#008B8B"),
                BarTextColor = Color.White
            };
            Application.Current.MainPage = page;
        }

        private void BHelp_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
