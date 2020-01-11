using System;

namespace DataAPI.Models
{
    public class PlantEntity
    {
        public Guid PlantID { get; }
        public string CommonName { get; }

        public string Description { get; }


        public PlantEntity(string commonName, string description)
            : this(Guid.Empty, commonName, description)
        {
        }

        public PlantEntity(Guid plantId, string commonName, string description)
        {
            PlantID = plantId;
            CommonName = commonName;
            Description = description;
        }
    }
}