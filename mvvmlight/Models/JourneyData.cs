using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace mvvmframework.Models
{
    public class JourneyData
    {
        public long JourneyId { get; set; }
        public int JourneyNumber { get; set; }
        public DateTime JourneyStartDate { get; set; }
        public DateTime JourneyEndDate { get; set; }
        [Ignore]
        public List<JourneyCoordinates> GPSData { get; set; }
    }
}
