using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FleeAndCatch_App.Droid;
using FleeAndCatch_App.sqlite;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_Android))]
namespace FleeAndCatch_App.Droid
{
    public class SQLite_Android :ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "fleeandcatch.db3";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            var connection = new SQLite.SQLiteConnection(path);
            return connection;
        }
    }
}