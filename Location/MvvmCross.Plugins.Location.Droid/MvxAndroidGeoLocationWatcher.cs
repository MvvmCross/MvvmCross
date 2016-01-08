// MvxAndroidGeoLocationWatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using System.Threading;
using Android.Content;
using Android.Locations;
using Android.OS;
using MvvmCross.Platform.Droid;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Plugins.Location.Droid
{
    [Obsolete("Use MvxAndroidLocationWatcher instead")]
    public sealed class MvxAndroidGeoLocationWatcher
        : MvxGeoLocationWatcher
        , IMvxLocationReceiver
    {
        private Context _context;
        private LocationManager _locationManager;
        private readonly MvxLocationListener _locationListener;

        public MvxAndroidGeoLocationWatcher()
        {
            EnsureStopped();
            _locationListener = new MvxLocationListener(this);
        }

        private Context Context => _context ?? (_context = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext);

        protected override void PlatformSpecificStart(MvxGeoLocationOptions options)
        {
            if (_locationManager != null)
                throw new MvxException("You cannot start the MvxLocation service more than once");

            _locationManager = (LocationManager) Context.GetSystemService(Context.LocationService);
            if (_locationManager == null)
            {
                MvxTrace.Warning( "Location Service Manager unavailable - returned null");
                SendError(MvxLocationErrorCode.ServiceUnavailable);
                return;
            }
            var criteria = new Criteria {Accuracy = options.EnableHighAccuracy ? Accuracy.Fine : Accuracy.Coarse};
            var bestProvider = _locationManager.GetBestProvider(criteria, true);
            if (bestProvider == null)
            {
                MvxTrace.Warning( "Location Service Provider unavailable - returned null");
                SendError(MvxLocationErrorCode.ServiceUnavailable);
                return;
            }
            // 4th September 2013 - defaults changed to 0,0 - meaning send updates as often as possible
            _locationManager.RequestLocationUpdates(bestProvider, 0, 0, _locationListener);
            // TODO - Ideally - _geoWatcher.MovementThreshold needed too
        }

        protected override void PlatformSpecificStop()
        {
            EnsureStopped();
        }

        private void EnsureStopped()
        {
            if (_locationManager != null)
            {
                _locationManager.RemoveUpdates(_locationListener);
                _locationManager = null;
            }
        }

        private static MvxGeoLocation CreateLocation(global::Android.Locations.Location androidLocation)
        {
            var position = new MvxGeoLocation {Timestamp = androidLocation.Time.FromMillisecondsUnixTimeToUtc()};
            var coords = position.Coordinates;

            if (androidLocation.HasAltitude)
                coords.Altitude = androidLocation.Altitude;

			if (androidLocation.HasBearing)
				coords.Heading = androidLocation.Bearing;

            coords.Latitude = androidLocation.Latitude;
            coords.Longitude = androidLocation.Longitude;
            if (androidLocation.HasSpeed)
                coords.Speed = androidLocation.Speed;
            if (androidLocation.HasAccuracy)
            {
                coords.Accuracy = androidLocation.Accuracy;
            }

            return position;
        }

        private static double HackReadValue(string testString, string key)
        {
            var startIndex = testString.IndexOf(key);
            var endIndex = testString.IndexOf(",", startIndex);
            var startPosition = startIndex + key.Length;
            var toParse = testString.Substring(startPosition, endIndex - startPosition);
            var value = double.Parse(toParse, CultureInfo.InvariantCulture);
            return value;
        }

        #region Implementation of ILocationListener

        public void OnLocationChanged(global::Android.Locations.Location androidLocation)
        {
            if (androidLocation == null)
            {
                MvxTrace.Trace("Android: Null location seen");
                return;
            }

            if (androidLocation.Latitude == double.MaxValue
                || androidLocation.Longitude == double.MaxValue)
            {
                MvxTrace.Trace("Android: Invalid location seen");
                return;
            }

            MvxGeoLocation location;
            try
            {
                location = CreateLocation(androidLocation);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Android: Exception seen in converting location " + exception.ToLongString());
                return;
            }

            SendLocation(location);
        }

        public void OnProviderDisabled(string provider)
        {
            SendError(MvxLocationErrorCode.ServiceUnavailable);
        }

        public void OnProviderEnabled(string provider)
        {
            // nothing to do 
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            switch (status)
            {
                case Availability.Available:
                    break;
                case Availability.OutOfService:
                    SendError(MvxLocationErrorCode.ServiceUnavailable);
                    break;
                case Availability.TemporarilyUnavailable:
                    SendError(MvxLocationErrorCode.PositionUnavailable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status));
            }
        }

        #endregion
    }
}