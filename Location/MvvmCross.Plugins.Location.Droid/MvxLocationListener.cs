// MvxLocationListener.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Locations;
using Android.OS;

namespace MvvmCross.Plugins.Location.Droid
{
    [Preserve(AllMembers = true)]
	public class MvxLocationListener
        : Java.Lang.Object
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