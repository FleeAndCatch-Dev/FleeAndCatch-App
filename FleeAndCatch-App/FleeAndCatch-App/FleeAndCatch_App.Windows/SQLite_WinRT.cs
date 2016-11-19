using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using FleeAndCatch_App.sqlite;
using FleeAndCatch_App.Windows;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_WinRT))]
namespace FleeAndCatch_App.Windows
{
    public class SQLite_WinRT : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "fleeandcatch.db3";
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            var connection = new SQLite.SQLiteConnection(path);
            return connection;
        }
    }
}
