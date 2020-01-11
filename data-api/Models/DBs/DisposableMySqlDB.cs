using System;
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
    }
}