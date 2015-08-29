﻿using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Touch;
using CoreLocation;
using Foundation;
using Cirrious.CrossCore.Touch.Platform;

namespace Cirrious.MvvmCross.Plugins.Location.Touch
{
    public sealed class MvxTouchLocationWatcher
        : MvxLocationWatcher
    {
        private CLLocationManager _locationManager;

		private MvxIosMajorVersionChecker _ios8VersionChecker;
		internal bool IsIOS8orHigher
		{
			get 
			{
				if (_ios8VersionChecker == null) 
				{
					_ios8VersionChecker = new MvxIosMajorVersionChecker (8);
				}
				return _ios8VersionChecker.IsVersionOrHigher;
			}
		}

        public MvxTouchLocationWatcher()
        {
            EnsureStopped();
        }

        protected override void PlatformSpecificStart(MvxLocationOptions options)
        {
            lock (this)
            {
                if (_locationManager != null)
                    throw new MvxException("You cannot start the MvxLocation service more than once");

                _locationManager = new CLLocationManager();
                _locationManager.Delegate = new LocationDelegate(this);

                if (options.MovementThresholdInM > 0)
                {
                    _locationManager.DistanceFilter = options.MovementThresholdInM;
                }
                else
                {
                    _locationManager.DistanceFilter = CLLocationDistance.FilterNone;
                }
                _locationManager.DesiredAccuracy = options.Accuracy == MvxLocationAccuracy.Fine ? CLLocation.AccuracyBest : CLLocation.AccuracyKilometer;
                if (options.TimeBetweenUpdates > TimeSpan.Zero)
                {
                    Mvx.Warning("TimeBetweenUpdates specified for MvxLocationOptions - but this is not supported in iOS");
                }


				if (options.TrackingMode == MvxLocationTrackingMode.Background)
				{
					if (IsIOS8orHigher)
					{
						_locationManager.RequestAlwaysAuthorization ();
					}
					else
					{
						Mvx.Warning ("MvxLocationTrackingMode.Background is not supported for iOS before 8");
					}
				}
				else
				{
					if (IsIOS8orHigher)
					{
						_locationManager.RequestWhenInUseAuthorization ();
					}
				}

                if (CLLocationManager.HeadingAvailable)
                    _locationManager.StartUpdatingHeading();

                _locationManager.StartUpdatingLocation();
            }
        }

        public override MvxGeoLocation CurrentLocation
        {
            get
            {
                if (_locationManager == null)
                    throw new MvxException("Location Manager not started");

                var iosLocation = _locationManager.Location;
                if (iosLocation == null)
                    return null;

                CLHeading heading = null;
                if (CLLocationManager.HeadingAvailable)
                    heading = _locationManager.Heading;

                return CreateLocation(iosLocation, heading);
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
                    if (CLLocationManager.HeadingAvailable)
                        _locationManager.StopUpdatingHeading();
                    _locationManager.Dispose();
                    _locationManager = null;
                }
            }
        }

        private static MvxGeoLocation CreateLocation(CLLocation location, CLHeading heading)
        {
            var position = new MvxGeoLocation { Timestamp = location.Timestamp.ToDateTimeUtc() };
            var coords = position.Coordinates;

            coords.Altitude = location.Altitude;
            coords.Latitude = location.Coordinate.Latitude;
            coords.Longitude = location.Coordinate.Longitude;
            coords.Speed = location.Speed;
            coords.Accuracy = location.HorizontalAccuracy;
            coords.AltitudeAccuracy = location.VerticalAccuracy;
            if (heading != null)
            {
                coords.Heading = heading.TrueHeading;
                coords.HeadingAccuracy = heading.HeadingAccuracy;
            }

            return position;
        }

        #region Nested type: LocationDelegate

        private class LocationDelegate : CLLocationManagerDelegate
        {
            private readonly MvxTouchLocationWatcher _owner;

            public LocationDelegate(MvxTouchLocationWatcher owner)
            {
                _owner = owner;
            }


            CLHeading _lastSeenHeading;

            public override void UpdatedHeading(CLLocationManager manager, CLHeading newHeading)
            {
                // note that we don't immediately send on the heading information.
                // for user's wanting real-time heading info a different plugin/api will be needed
                _lastSeenHeading = newHeading;
            }

            public override void LocationsUpdated(CLLocationManager manager, CLLocation[] locations)
            {
                // see https://github.com/slodge/MvvmCross/issues/92 and http://stackoverflow.com/questions/13262385/monotouch-cllocationmanagerdelegate-updatedlocation
                if (locations.Length == 0)
                {
                    MvxTrace.Error("iOS has passed LocationsUpdated an empty array - this should never happen");
                    return;
                }

                var mostRecent = locations[locations.Length - 1];
                var converted = CreateLocation(mostRecent, _lastSeenHeading);
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

			public override void AuthorizationChanged (CLLocationManager manager, CLAuthorizationStatus status)
			{
				switch (status) {
				case CLAuthorizationStatus.NotDetermined:
					_owner.Permission = MvxLocationPermission.Unknown;
					break;
				case CLAuthorizationStatus.Restricted:
				case CLAuthorizationStatus.Denied:
					_owner.Permission = MvxLocationPermission.Denied;
					break;
				case CLAuthorizationStatus.AuthorizedAlways:
				case CLAuthorizationStatus.AuthorizedWhenInUse:
					_owner.Permission = MvxLocationPermission.Granted;
					break;
				default:
					throw new ArgumentOutOfRangeException ();
				}
			}
        }

        #endregion
    }
}