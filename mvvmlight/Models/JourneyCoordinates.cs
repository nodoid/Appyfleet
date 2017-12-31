using SQLite.Net.Attributes;
using System.Collections.Generic;

namespace mvvmframework.Models
{
    public class JourneyCoordinates
    {
        public long JourneyId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class JourneyCoordinatesModel
    {
        public JourneyCoordinatesModel()
        {
            Status = new StatusModel();
            GeoCoordinates = new List<JourneyCoordinates>();
        }

        public List<JourneyCoordinates> GeoCoordinates { get; set; }
        public StatusModel Status { get; set; }
    }
}
