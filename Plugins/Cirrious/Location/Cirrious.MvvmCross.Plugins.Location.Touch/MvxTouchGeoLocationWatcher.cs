﻿// MvxTouchGeoLocationWatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Touch;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using Cirrious.CrossCore.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins.Location.Touch
{
    public sealed class MvxTouchGeoLocationWatcher
        : MvxGeoLocationWatcher
    {
        private CLLocationManager _locationManager;

        public MvxTouchGeoLocationWatcher()
        {
            EnsureStopped();
        }

        protected override void PlatformSpecificStart(MvxGeoLocationOptions options)
        {
            lock (this)
            {
                if (_locationManager != null)
                    throw new MvxException("You cannot start the MvxLocation service more than once");

                _locationManager = new CLLocationManager();
                _locationManager.Delegate = new LocationDelegate(this);

                //_locationManager.DesiredAccuracy = options.EnableHighAccuracy ? Accuracy.Fine : Accuracy.Coarse;
                _locationManager.StartUpdatingLocation();
            }
        }

        protected override void SendLocation(MvxGeoLocation location)
        {
            // note - no need to lock here - just check then go
            if (_locationManager == null)
                return;

            base.SendLocation(location);
        }

        protected override void PlatformSpecificStop()
        {
            EnsureStopped();
        }

        private void EnsureStopped()
        {
            lock (this)
            {
                if (_locationManager != null)
                {
                    _locationManager.Delegate = null;
                    _locationManager.StopUpdatingLocation();
					_locationManager.Dispose();
                    _locationManager = null;
                }
            }
        }

        private static MvxGeoLocation CreateLocation(CLLocation location)
        {
            var position = new MvxGeoLocation {Timestamp = location.Timestamp.ToDateTimeUtc()};
            var coords = position.Coordinates;

            coords.Altitude = location.Altitude;
            coords.Latitude = location.Coordinate.Latitude;
            coords.Longitude = location.Coordinate.Longitude;
            coords.Speed = location.Speed;
            coords.Accuracy = location.HorizontalAccuracy;
            coords.AltitudeAccuracy = location.VerticalAccuracy;

            return position;
        }

        #region Nested type: LocationDelegate

        private class LocationDelegate : CLLocationManagerDelegate
        {
            private readonly MvxTouchGeoLocationWatcher _owner;

            public LocationDelegate(MvxTouchGeoLocationWatcher owner)
            {
                _owner = owner;
            }

			public override void LocationsUpdated (CLLocationManager manager, CLLocation[] locations)
			{
				// see https://github.com/slodge/MvvmCross/issues/92 and http://stackoverflow.com/questions/13262385/monotouch-cllocationmanagerdelegate-updatedlocation
				if (locations.Length == 0)
				{
					MvxTrace.Error("iOS has passed LocationsUpdated an empty array - this should never happen");
					return;
				}

				var mostRecent = locations[locations.Length - 1];
				var converted = CreateLocation(mostRecent);
				_owner.SendLocation(converted);
			}

            public override void Failed(CLLocationManager manager, NSError error)
            {
                // ignored for now
            }

            public override void MonitoringFailed(CLLocationManager manager, CLRegion region, NSError error)
            {
                // ignored for now
            }
        }

        #endregion
    }
}