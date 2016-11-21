using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace FleeAndCatch_App.sqlite.database
{
    public class Connection
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Address { get; set; }
        public bool Save { get; set; }
    }

    public class ConnectionDB
    {
        private SQLiteConnection _connection;

        public ConnectionDB()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<Connection>();
        }

        public List<Connection> GetConnections()
        {
            return (from t in _connection.Table<Connection>() select t).ToList();
        }

        public Connection GetConnection(int pId)
        {
            return _connection.Table<Connection>().FirstOrDefault(t => t.Id == pId);
        }
        public Connection GetConnection(string pAddress)
        {
            return _connection.Table<Connection>().FirstOrDefault(t => t.Address == pAddress);
        }

        public void DeleteConnection(int pId)
        {
            _connection.Delete<Connection>(pId);
        }

        public void AddConnection(string pAddress, bool pSave)
        {
            var connection = new Connection
            {
                Address = pAddress,
                Save = pSave
            };
            _connection.Insert(connection);
        }

        public void UpdateConnection(Connection pConnection)
        {
            _connection.Update(pConnection);
        }
    }
}
