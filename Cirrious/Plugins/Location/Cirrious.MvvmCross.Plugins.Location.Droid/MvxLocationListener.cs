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