using System;

namespace Cirrious.MvvmCross.Interfaces.Platform.Location
{
    public class MvxGeoLocation
    {
        public MvxCoordinates Coordinates { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public MvxGeoLocation()
        {
            Coordinates = new MvxCoordinates();
        }
    }
}