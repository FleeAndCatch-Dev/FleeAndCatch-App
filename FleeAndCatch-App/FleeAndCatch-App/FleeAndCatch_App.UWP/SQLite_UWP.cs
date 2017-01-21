using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using FleeAndCatch_App.SQLite;
using FleeAndCatch_App.UWP;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_UWP))]
namespace FleeAndCatch_App.UWP
{
    public class SQLite_UWP : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "fleeandcatch.db3";
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            var connection = new SQLiteConnection(path);
            return connection;
        }
    }
}
