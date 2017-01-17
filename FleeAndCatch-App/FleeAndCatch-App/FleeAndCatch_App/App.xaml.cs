using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FleeAndCatch_App.PageModels;
using FleeAndCatch_App.SQLite;
using FleeAndCatch_App.SQLite.Database;
using Xamarin.Forms;

namespace FleeAndCatch_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var page = FreshMvvm.FreshPageModelResolver.ResolvePageModel<SignInPageModel>();
            var navigation = new FreshMvvm.FreshNavigationContainer(page)
            {
                BarBackgroundColor = Color.FromHex("#008B8B"),
                BarTextColor = Color.White
            };
            MainPage = navigation;
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
