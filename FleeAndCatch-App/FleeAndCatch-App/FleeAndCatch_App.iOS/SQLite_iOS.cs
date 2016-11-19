using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FleeAndCatch_App.iOS;
using FleeAndCatch_App.sqlite;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace FleeAndCatch_App.iOS
{
    public class SQLite_iOS : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "fleeandcatch.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFilename);
            var connection = new SQLite.SQLiteConnection(path);
            return connection;
        }
    }
}
