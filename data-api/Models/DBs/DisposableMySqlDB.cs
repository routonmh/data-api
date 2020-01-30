using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DataAPI.Models.DBs
{
    public abstract class DisposableMySqlDB : IDisposable
    {
        public MySqlConnection Connection { get; set; }

        public void Dispose()
        {
            Connection.CloseAsync();
        }

        public abstract Task OpenConnectionAsync();
        public abstract MySqlCommand CreateNewCommand();
    }
}