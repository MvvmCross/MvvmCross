#region Copyright

// <copyright file="MvxLocationListener.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Android.Locations;
using Android.OS;

namespace Cirrious.MvvmCross.Plugins.Location.Droid
{
    public class MvxLocationListener
        : Java.Lang.Object
          , ILocationListener
    {
        private readonly MvxAndroidGeoLocationWatcher _owner;

        public MvxLocationListener(MvxAndroidGeoLocationWatcher owner)
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