using System;
using System.Collections.Generic;
using mvvmframework.Helpers;

namespace mvvmframework.Models
{
    public class GPSData
    {
        public int id { get; set; }
        public double Longitude { get; set; }
        public double Latitiude { get; set; }
        public double Speed { get; set; }
        public double SpeedInKnots => Speed.KmhToKnots();
        public Dictionary<DateTime, string> EventOccurred { get; set; }
    }
}
