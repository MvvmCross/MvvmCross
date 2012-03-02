using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Services.Location;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using MonoTouch.CoreLocation;

namespace Cirrious.MvvmCross.Touch.Platform.Location
{
    public sealed class MvxTouchGeoLocationWatcher : MvxBaseGeoLocationWatcher
    {
        private CLLocationManager _locationManager;

        public MvxTouchGeoLocationWatcher()
        {
            EnsureStopped();
        }

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

            public override void Failed(CLLocationManager manager, MonoTouch.Foundation.NSError error)
            {
#warning TODO!
                //base.Failed(manager, error);
            }

            public override void MonitoringFailed(CLLocationManager manager, CLRegion region, MonoTouch.Foundation.NSError error)
            {
#warning TODO!
                //base.MonitoringFailed(manager, region, error);
            }
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
    }
}
