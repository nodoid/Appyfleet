using System;

namespace mvvmframework
{
    public class LocationDetails
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime TimeStamp { get; set; }

        public LocationDetails()
        {

        }

        public LocationDetails(double lat, double lon,  DateTime stamp)
        {
            Latitude = lat;
            Longitude = lon;
            TimeStamp = stamp;
        }
    }
}
