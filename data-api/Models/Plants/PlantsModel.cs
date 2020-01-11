using System;
using System.Data.Common;
using System.Threading.Tasks;
using DataAPI.Models.DBs;
using MySql.Data.MySqlClient;

namespace DataAPI.Models
{
    public class PlantsModel
    {
        public static async Task<PlantEntity> GetPlantByID(Guid plantId)
        {
            PlantEntity plant = null;

            using (LocalDB db = new LocalDB())
            {
                await db.Connection.OpenAsync();

                MySqlCommand cmd = db.Connection.CreateCommand();

                string query = "SELECT CommonName, Description FROM plant " +
                               "WHERE PlantID = @PlantID";

                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.CommandText = query;

                DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    string commonName = reader["CommonName"] as string ?? null; // Cleanly catch cast error to null
                    string description = reader["Description"] as string ?? null;

                    plant = new PlantEntity(commonName, description);
                }
            }

            return plant;
        }
    }
}