using System;
using Microsoft.Xna.Framework;

namespace Phone7.Fx.Location
{
    public class Distance
    {
        private const int RADIUS = 6366;

        public static double GetDistance(System.Device.Location.GeoCoordinate g1, System.Device.Location.GeoCoordinate g2)
        {
            return GetDistance(g1.Latitude, g1.Longitude, g2.Latitude, g2.Longitude);
        }

        public static double GetDistanceLowAccuracy(System.Device.Location.GeoCoordinate g1, System.Device.Location.GeoCoordinate g2)
        {
            return GetDistanceLowAccuracy(g1.Latitude, g1.Longitude, g2.Latitude, g2.Longitude);
        }

        public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // d=2*asin(sqrt((sin((lat1-lat2)/2))^2 + cos(lat1)*cos(lat2)*(sin((lon1- lon2)/2))^2))

            ConvertToRadians(ref lat1, ref lon1, ref lat2, ref lon2);

            return 2 * Math.Asin(Math.Sqrt(Math.Pow((Math.Sin((lat1 - lat2) / 2)), 2) + Math.Cos(lat1) * Math.Cos(lat2) * (Math.Pow(Math.Sin(((lon1 - lon2) / 2)), 2)))) * RADIUS;
        }

        public static double GetDistanceLowAccuracy(double lat1, double lon1, double lat2, double lon2)
        {
            // d=acos(sin(lat1)*sin(lat2)+cos(lat1)*cos(lat2)*cos(lon1-lon2))

            ConvertToRadians(ref lat1, ref lon1, ref lat2, ref lon2);

            return Math.Acos(Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lon1 - lon2)) * RADIUS;


        }

        private static void ConvertToRadians(ref double lat1, ref double lon1, ref double lat2, ref double lon2)
        {
            lat1 = MathHelper.ToRadians((float)lat1);
            lon1 = MathHelper.ToRadians((float)lon1);
            lat2 = MathHelper.ToRadians((float)lat2);
            lon2 = MathHelper.ToRadians((float)lon2);
        }

       
    }
}