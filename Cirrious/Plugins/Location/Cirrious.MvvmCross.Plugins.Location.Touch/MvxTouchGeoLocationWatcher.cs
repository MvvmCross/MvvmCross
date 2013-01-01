// MvxTouchGeoLocationWatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Plugins.Location.Touch
{
    public sealed class MvxTouchGeoLocationWatcher
        : MvxBaseGeoLocationWatcher
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
#warning Why is _locationManager not disposed here? I seem to remember it was because of a crash problem!
                    //_locationManager.Dispose();
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


#warning - see https://github.com/slodge/MvvmCross/issues/92 and http://stackoverflow.com/questions/13262385/monotouch-cllocationmanagerdelegate-updatedlocation
            [Obsolete]
            public override void UpdatedLocation(CLLocationManager manager, CLLocation newLocation,
                                                 CLLocation oldLocation)
            {
                _owner.SendLocation(CreateLocation(newLocation));
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