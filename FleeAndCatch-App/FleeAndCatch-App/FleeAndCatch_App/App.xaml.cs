using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FleeAndCatch_App.pages;
using Xamarin.Forms;

namespace FleeAndCatch_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            /*var page = new NavigationPage(new SignIn())
            {
                BarBackgroundColor = Color.FromHex("#6495ED"),
                BarTextColor = Color.White
            };
            MainPage = page;*/
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
