using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FleeAndCatch_App.SQLite.Database
{
    public class ConnectionDB
    {
        private SQLiteConnection connection;

        public ConnectionDB()
        {
            connection = DependencyService.Get<ISQLite>().GetConnection();
            connection.CreateTable<Connection>();
        }

        public List<Connection> GetConnections()
        {
            return (from t in connection.Table<Connection>() select t).ToList();
        }

        public Connection GetConnection(int pId)
        {
            return connection.Table<Connection>().FirstOrDefault(t => t.Id == pId);
        }
        public Connection GetConnection(string pAddress)
        {
            return connection.Table<Connection>().FirstOrDefault(t => t.Address == pAddress);
        }

        public void DeleteConnection(int pId)
        {
            connection.Delete<Connection>(pId);
        }

        public void AddConnection(string pAddress, bool pSave)
        {
            var connection = new Connection
            {
                Address = pAddress,
                Save = pSave
            };
            this.connection.Insert(connection);
        }

        public void UpdateConnection(Connection pConnection)
        {
            connection.Update(pConnection);
        }
    }

    public class Connection : Models.ConnectionModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
