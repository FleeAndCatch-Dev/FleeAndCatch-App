using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using FleeAndCatch_App.sqlite;
using FleeAndCatch_App.WinPhone;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_WinPhone))]
namespace FleeAndCatch_App.WinPhone
{
    public class SQLite_WinPhone : ISQLite
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
