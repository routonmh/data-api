using System;
using MySql.Data.MySqlClient;

namespace DataAPI.Models.DBs
{
    public class LocalDB : DisposableMySqlDB
    {
        public LocalDB()
        {
            string connectionString;

            string environmentIndicator = Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT");
            if (!String.IsNullOrEmpty(environmentIndicator) &&
                environmentIndicator.Equals("Development"))
                // Use Development DB
                connectionString = Environment.GetEnvironmentVariable(
                    "LOCAL_DEV_DB_CONNECTION");
            else
            {
                // Use Production DB
                connectionString = Environment
                    .GetEnvironmentVariable("LOCAL_DB_CONNECTION");
            }


            Connection = new MySqlConnection(connectionString);
        }
    }
}