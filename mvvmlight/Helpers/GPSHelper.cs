using System;
using System.Globalization;
using mvvmframework.Models;

namespace mvvmframework.Helpers
{
    public static class GPSHelper
    {
        public static double DistanceTo(this LocationServiceData from, LocationServiceData to)
        {
            return haversine_mi(from.Latitude, from.Longitude, to.Latitude, to.Longitude);
        }

        public static float DistanceBetween(this LocationServiceData dta, double[] data)
        {
            return (float)haversine_mi(data[0], data[1], data[2], data[3]);
        }

        public static double haversine_mi(double lat1, double long1, double lat2, double long2)
        {
            var d2r = (Math.PI / 180.0);
            var dlong = (long2 - long1) * d2r;
            var dlat = (lat2 - lat1) * d2r;
            var a = Math.Pow(Math.Sin(dlat / 2.0), 2) + Math.Cos(lat1 * d2r) * Math.Cos(lat2 * d2r) * Math.Pow(Math.Sin(dlong / 2.0), 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return 3956 * c;
        }

        public static string ToDegreesMinutesSeconds(double decVal, GPSCoordinate coord)
        {
            var nfi = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };

            var _ = string.Empty;

            var num1 = Math.Abs((int)decVal);
            var num = Math.Abs(num1);
            var single1 = (float)(60 * (Math.Abs(decVal) - (double)num1));
            var single = Math.Abs(single1);

            if (coord == GPSCoordinate.Latitude)
            {
                if (decVal >= 0)
                {
                    _ = num.ToString("00", nfi);
                    return string.Concat(_, single.ToString("00.0000", nfi), "N");
                }
                _ = num.ToString("00", nfi);
                return string.Concat(_, single.ToString("00.0000", nfi), "S");
            }
            if (coord != GPSCoordinate.Longitude)
            {
                return null;
            }
            if (decVal >= 0)
            {
                _ = num.ToString("000", nfi);
                return string.Concat(_, single.ToString("00.0000,", nfi), "E");
            }
            _ = num.ToString("000", nfi);
            return string.Concat(_, single.ToString("00.0000", nfi), "W");
        }

        public static long TimeForTimeStamp(long ticks)
        {
            TimeSpan dateTime = new DateTime(ticks) - new DateTime(2000, 1, 1);
            return (long)dateTime.TotalSeconds;
        }
    }
}
