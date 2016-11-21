using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace FleeAndCatch_App.sqlite
{
    public interface ISQLite
    {
        /// <summary>
        /// Returns a SQLConnection from the local database.
        /// </summary>
        /// <returns>SQLConnection</returns>
        SQLiteConnection GetConnection();
    }
}
