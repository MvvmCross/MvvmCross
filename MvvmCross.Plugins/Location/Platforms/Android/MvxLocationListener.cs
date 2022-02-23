// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Locations;
using Android.OS;
using Java.Lang;

namespace MvvmCross.Plugin.Location.Platforms.Android
{
    [Preserve(AllMembers = true)]
    public class MvxLocationListener
        : Object
        , ILocationListener
    {
        private readonly IMvxLocationReceiver _owner;

        public MvxLocationListener(IMvxLocationReceiver owner)
        {
            _owner = owner;
        }

        #region Implementation of ILocationListener

        public void OnLocationChanged(global::Android.Locations.Location location)
        {
            _owner.OnLocationChanged(location);
        }

        public void OnProviderDisabled(string provider)
        {
            _owner.OnProviderDisabled(provider);
        }

        public void OnProviderEnabled(string provider)
        {
            _owner.OnProviderEnabled(provider);
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            _owner.OnStatusChanged(provider, status, extras);
        }

        #endregion
    }
}
