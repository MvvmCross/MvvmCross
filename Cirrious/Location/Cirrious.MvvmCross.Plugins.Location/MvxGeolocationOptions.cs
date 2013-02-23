// MvxGeolocationOptions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Plugins.Location
{
    public class MvxGeoLocationOptions
    {
        public int Timeout { get; set; }
        public int MaximumAge { get; set; }
        public bool EnableHighAccuracy { get; set; }
    }
}