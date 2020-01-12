using System;

namespace DataAPI.Models
{
    public class PlantEntity
    {
        public Guid PlantID { get; }
        public string CommonName { get; }
        public string ScientificName { get; }

        public string Description { get; }

        public PlantEntity(Guid plantId, string commonName, string scientificName, string description)
        {
            PlantID = plantId;
            CommonName = commonName;
            ScientificName = scientificName;
            Description = description;
        }
    }
}