using System;

namespace DataAPI.Models
{
    public class PlantReportedLocation
    {
        public Guid LocationID { get; }
        public Guid PlantID { get; }
        public decimal Latitude { get; }
        public decimal Longitude { get; }

        public PlantReportedLocation(Guid locationID, Guid plantID, decimal latitude, decimal longitude)
        {
            LocationID = locationID;
            PlantID = PlantID;
            Latitude = latitude;
            Longitude = Longitude;
        }
    }
}