// MvxAndroidLocationWatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Android.Content;
using Android.Locations;
using Android.OS;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Plugins.Location.Droid
{
    public sealed class MvxAndroidLocationWatcher
        : MvxLocationWatcher
          , IMvxLocationReceiver
    {
        private Context _context;
        private LocationManager _locationManager;
        private List<string> _enabledProviders;
        private readonly MvxLocationListener _locationListener;

        public MvxAndroidLocationWatcher ()
        {
            EnsureStopped ();
            _locationListener = new MvxLocationListener (this);
        }

        private Context Context {
            get {
                if (_context == null) {
                    _context = Mvx.Resolve<IMvxAndroidGlobals> ().ApplicationContext;
                }
                return _context;
            }
        }

        protected override void PlatformSpecificStart (MvxLocationOptions options)
        {
            if (_locationManager != null)
                throw new MvxException ("You cannot start the MvxLocation service more than once");

            _locationManager = (LocationManager)Context.GetSystemService (Context.LocationService);
            if (_locationManager == null) {
                MvxTrace.Warning ("Location Service Manager unavailable - returned null");
                SendError (MvxLocationErrorCode.ServiceUnavailable);
                return;
            }
            
            _enabledProviders = _locationManager.AllProviders
				.Where ((provider) => _locationManager.IsProviderEnabled (provider) && provider != LocationManager.PassiveProvider)
				.ToList ();

            if (_enabledProviders.Count == 0) {
                MvxTrace.Warning ("Location Service Provider unavailable");
                SendError (MvxLocationErrorCode.ServiceUnavailable);
            }

            _locationManager.RequestLocationUpdates (
                LocationManager.NetworkProvider,
                (long)options.TimeBetweenUpdates.TotalMilliseconds,
                options.MovementThresholdInM, 
                _locationListener);

            _locationManager.RequestLocationUpdates (
                LocationManager.GpsProvider,
                (long)options.TimeBetweenUpdates.TotalMilliseconds,
                options.MovementThresholdInM, 
                _locationListener);
        }

        protected override void PlatformSpecificStop ()
        {
            EnsureStopped ();
        }

        private void EnsureStopped ()
        {
            if (_locationManager != null) {
                _locationManager.RemoveUpdates (_locationListener);
                _locationManager = null;
                _enabledProviders.Clear ();
            }
        }

        private static MvxGeoLocation CreateLocation (global::Android.Locations.Location androidLocation)
        {
            var position = new MvxGeoLocation { Timestamp = androidLocation.Time.FromMillisecondsUnixTimeToUtc () };
            var coords = position.Coordinates;

            if (androidLocation.HasAltitude)
                coords.Altitude = androidLocation.Altitude;

            if (androidLocation.HasBearing)
                coords.Heading = androidLocation.Bearing;

            coords.Latitude = androidLocation.Latitude;
            coords.Longitude = androidLocation.Longitude;
            if (androidLocation.HasSpeed)
                coords.Speed = androidLocation.Speed;
            if (androidLocation.HasAccuracy) {
                coords.Accuracy = androidLocation.Accuracy;
            }

            return position;
        }

        public override MvxGeoLocation CurrentLocation { 
            get {
                if (_locationManager == null)
                    throw new MvxException ("Location Manager not started");

                var androidLocation = _locationManager.GetLastKnownLocation (LocationManager.GpsProvider);
                if (androidLocation == null)
                    androidLocation = _locationManager.GetLastKnownLocation (LocationManager.NetworkProvider);
					
                if (androidLocation == null)
                    return null;

                return CreateLocation (androidLocation);
            }
        }

        #region Implementation of ILocationListener

        public void OnLocationChanged (global::Android.Locations.Location androidLocation)
        {
            if (androidLocation == null) {
                MvxTrace.Trace ("Android: Null location seen");
                return;
            }

            if (androidLocation.Latitude == double.MaxValue
                || androidLocation.Longitude == double.MaxValue) {
                MvxTrace.Trace ("Android: Invalid location seen");
                return;
            }

            MvxGeoLocation location;
            try {
                location = CreateLocation (androidLocation);
            } catch (ThreadAbortException) {
                throw;
            } catch (Exception exception) {
                MvxTrace.Trace ("Android: Exception seen in converting location " + exception.ToLongString ());
                return;
            }

            SendLocation (location);
        }

        public void OnProviderDisabled (string provider)
        {
            _enabledProviders.Remove (provider);
            if (_enabledProviders.Count == 0)
                SendError (MvxLocationErrorCode.PositionUnavailable);
        }

        public void OnProviderEnabled (string provider)
        {
            _enabledProviders.Add (provider);
        }

        public void OnStatusChanged (string provider, Availability status, Bundle extras)
        {
            switch (status) {
            case Availability.Available:
                break;
            case Availability.OutOfService:
            case Availability.TemporarilyUnavailable:
                SendError (MvxLocationErrorCode.PositionUnavailable);
                break;
            default:
                throw new ArgumentOutOfRangeException ("status");
            }
        }

        #endregion
    }
}