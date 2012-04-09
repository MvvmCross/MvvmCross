#region Copyright
// <copyright file="MvxTouchGeoLocationWatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Platform.Location;
using Cirrious.MvvmCross.Platform.Location;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;

namespace Cirrious.MvvmCross.Touch.Platform.Location
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
	            
	#warning TODO DesiredAccuracy! Plus movement threshold?
	            //_locationManager.DesiredAccuracy = options.EnableHighAccuracy ? Accuracy.Fine : Accuracy.Coarse;
	            _locationManager.StartUpdatingLocation();
			}
        }
		
		protected override void SendLocation (MvxGeoLocation location)
		{
			// note - no need to lock here - just check then go
			if (_locationManager == null)
				return;
			
			base.SendLocation (location);
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
            var position = new MvxGeoLocation { Timestamp = location.Timestamp.ToDateTimeUtc() };
            var coords = position.Coordinates;

#warning should some of these coords fields be nullable?
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
            private MvxTouchGeoLocationWatcher _owner;

            public LocationDelegate(MvxTouchGeoLocationWatcher owner)
            {
                _owner = owner;
            }
			
            public override void UpdatedLocation(CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
            {
                _owner.SendLocation(CreateLocation(newLocation));
            }

            public override void Failed(CLLocationManager manager, NSError error)
            {
#warning TODO!
                //base.Failed(manager, error);
            }

            public override void MonitoringFailed(CLLocationManager manager, CLRegion region, NSError error)
            {
#warning TODO!
                //base.MonitoringFailed(manager, region, error);
            }
        }

        #endregion
    }
}
