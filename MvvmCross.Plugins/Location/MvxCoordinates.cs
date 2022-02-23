// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Location
{
    [Preserve(AllMembers = true)]
    public class MvxCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Accuracy { get; set; }

        public double? Altitude { get; set; }
        public double? AltitudeAccuracy { get; set; }

        public double? Heading { get; set; }
        public double? HeadingAccuracy { get; set; }

        public double? Speed { get; set; }
    }
}
