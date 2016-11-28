using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Communication;
using FleeAndCatch_App.pages;
using FleeAndCatch_App.sqlite;
using FleeAndCatch_App.sqlite.database;
using Xamarin.Forms;

namespace FleeAndCatch_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var page = new NavigationPage(new SignIn())
            {
                BarBackgroundColor = Color.FromHex("#008B8B"),
                BarTextColor = Color.White
            };
            MainPage = page;
        }

        protected override void OnStart()
        {
            SQLiteDB.Connection = new ConnectionDB();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
