using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DataAPI.Models.DBs
{
    public class LocalDB : DisposableMySqlDB
    {
        public LocalDB()
        {
            string connectionString;

            // Get connection string from environment variable.
            connectionString = Environment
                .GetEnvironmentVariable("LOCAL_DB_CONNECTION");

            Connection = new MySqlConnection(connectionString);
        }

        public override Task OpenConnectionAsync()
        {
            return Connection.OpenAsync();
        }

        public override MySqlCommand CreateNewCommand()
        {
            return Connection.CreateCommand();
        }
    }
}