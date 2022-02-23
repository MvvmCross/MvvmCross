// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Locations;
using Android.OS;

namespace MvvmCross.Plugin.Location.Platforms.Android
{
    public interface IMvxLocationReceiver
    {
        void OnLocationChanged(global::Android.Locations.Location location);
        void OnProviderDisabled(string provider);
        void OnProviderEnabled(string provider);
        void OnStatusChanged(string provider, Availability status, Bundle extras);
    }
}
