using System;
using System.Data.Common;
using System.Threading.Tasks;
using DataAPI.Models.DBs;
using MySql.Data.MySqlClient;

namespace DataAPI.Models
{
    public static class PlantsModel
    {
        public static async Task<PlantEntity> GetPlantByID(Guid plantId)
        {
            PlantEntity plant = null;

            using (LocalDB db = new LocalDB())
            {
                await db.Connection.OpenAsync();

                MySqlCommand cmd = db.Connection.CreateCommand();

                string query = "SELECT PlantID, CommonName, ScientificName, Description FROM plant_entity " +
                               "WHERE PlantID = @PlantID";

                cmd.Parameters.AddWithValue("@PlantID", plantId);
                cmd.CommandText = query;

                DbDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    Guid plantID = reader["PlantID"] as Guid? ?? Guid.Empty;
                    string commonName = reader["CommonName"] as string ?? null; // Cleanly catch cast error to null
                    string scientificName = reader["ScientificName"] as string ?? null;
                    string description = reader["Description"] as string ?? null;

                    plant = new PlantEntity(plantID, commonName, scientificName, description);
                }
            }

            return plant;
        }
    }
}