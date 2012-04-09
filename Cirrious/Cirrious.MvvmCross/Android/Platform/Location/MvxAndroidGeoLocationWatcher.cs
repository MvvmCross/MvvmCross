#region Copyright
// <copyright file="MvxAndroidGeoLocationWatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#warning Credit to ChrisNTR fro some of this code

using System;
using System.Globalization;
using System.Threading;
using Android.Content;
using Android.Locations;
using Android.OS;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Location;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Platform.Location;

namespace Cirrious.MvvmCross.Android.Platform.Location
{
    public sealed class MvxAndroidGeoLocationWatcher 
        : MvxBaseGeoLocationWatcher
        , ILocationListener
        , IMvxServiceConsumer<IMvxAndroidGlobals>
    {
        private Context _context;
        private LocationManager _locationManager;

        public MvxAndroidGeoLocationWatcher()
        {
            EnsureStopped();
        }

        private Context Context
        {
            get
            {
                if (_context == null)
                {
                    _context = this.GetService<IMvxAndroidGlobals>().ApplicationContext;
                }
                return _context;
            }
        }

        protected override void PlatformSpecificStart(MvxGeoLocationOptions options)
        {
            if (_locationManager != null)
                throw new MvxException("You cannot start the MvxLocation service more than once");

            _locationManager = (LocationManager)Context.GetSystemService(Context.LocationService);
            var criteria = new Criteria() { Accuracy = options.EnableHighAccuracy ? Accuracy.Fine : Accuracy.Coarse };
            var bestProvider = _locationManager.GetBestProvider(criteria, true);
            _locationManager.RequestLocationUpdates(bestProvider, 5000, 2, this);
#warning _geoWatcher.MovementThreshold needed too
        }

        protected override void PlatformSpecificStop()
        {
            EnsureStopped();
        }

        private void EnsureStopped()
        {
            if (_locationManager != null)
            {
                _locationManager.RemoveUpdates(this);
                _locationManager = null;
            }
        }

        private static MvxGeoLocation CreateLocation(global::Android.Locations.Location androidLocation)
        {
            var position = new MvxGeoLocation { Timestamp = androidLocation.Time.FromMillisecondsUnixTimeToUtc() };
            var coords = position.Coordinates;

#warning should some of these coords fields be nullable?
            if (androidLocation.HasAltitude)
                coords.Altitude = androidLocation.Altitude;

#warning MONODROID BUG WORKAROUND!
#warning MONODROID BUG WORKAROUND!
#warning MONODROID BUG WORKAROUND!
#warning MONODROID BUG WORKAROUND!
#warning MONODROID BUG WORKAROUND!
#warning MONODROID BUG WORKAROUND!
#warning MONODROID BUG WORKAROUND!
#warning MONODROID BUG WORKAROUND!
#warning MONODROID BUG WORKAROUND!
#warning MONODROID BUG WORKAROUND!
#warning MONODROID BUG WORKAROUND!
            var testString = androidLocation.ToString();
            coords.Latitude = HackReadValue(testString, "mLatitude=");
            coords.Longitude = HackReadValue(testString, "mLongitude=");

            return position;
            /*
            coords.Latitude = androidLocation.Latitude;
            coords.Longitude = androidLocation.Longitude;
            if (androidLocation.HasSpeed)
                coords.Speed = androidLocation.Speed;
            if (androidLocation.HasAccuracy)
            {
                coords.Accuracy = androidLocation.Accuracy;
            }

#warning what to do with coords.AltitudeAccuracy ?S
            //coords.AltitudeAccuracy = androidLocation.Accuracy;

            return position;
             */
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
            SendError(MvxLocationErrorCode.PositionUnavailable);
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
                case Availability.TemporarilyUnavailable:
                    SendError(MvxLocationErrorCode.PositionUnavailable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("status");
            }
        }

        #endregion
    }
}
