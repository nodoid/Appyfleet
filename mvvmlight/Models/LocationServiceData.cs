using SQLite.Net.Attributes;
using System;

namespace mvvmframework.Models
{
    public class LocationServiceData
    {
        public int id { get; set; }
        public int datapoint { get; set; }
        public double Accuracy { get; set; }
        public double Altitude { get; set; }
        public double AltitudeAccuracy { get; set; }
        public double Heading { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }

        [Ignore]
        public DateTimeOffset TimeStamp { get; set; }

        public DateTime TimeStampDate { get => TimeStamp.DateTime; }
        public double TimeStampOffset { get => TimeStamp.Offset.TotalSeconds; }

        public string EventName { get; set; }
    }
}
