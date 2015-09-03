// MvxGeoLocation.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Plugins.Location
{
    public class MvxGeoLocation
    {
        public MvxGeoLocation()
        {
            Coordinates = new MvxCoordinates();
        }

        public MvxCoordinates Coordinates { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}