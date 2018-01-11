// MvxCoordinates.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Plugins.Location
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