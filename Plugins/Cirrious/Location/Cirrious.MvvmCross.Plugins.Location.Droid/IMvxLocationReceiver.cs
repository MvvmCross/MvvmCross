// IMvxLocationReceiver.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Locations;
using Android.OS;

namespace Cirrious.MvvmCross.Plugins.Location.Droid
{
    public interface IMvxLocationReceiver
    {
        void OnLocationChanged(global::Android.Locations.Location location);
        void OnProviderDisabled(string provider);
        void OnProviderEnabled(string provider);
        void OnStatusChanged(string provider, Availability status, Bundle extras);
    }
}