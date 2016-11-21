using System;
using Communication;
using Xamarin.Forms;

namespace FleeAndCatch_App.pages.master
{
    public partial class Menu : MasterDetailPage
    {
        private Client _client;

        public Menu(Client pClient)
        {
            InitializeComponent();

            this._client = pClient;
        }

        private void BLogOut_OnClicked(object sender, EventArgs e)
        {
            _client.Close();
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
