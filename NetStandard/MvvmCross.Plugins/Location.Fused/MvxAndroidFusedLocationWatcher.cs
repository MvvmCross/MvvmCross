﻿using System;
using System.Threading;
using Android.Content;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Plugins.Location.Fused.Droid
{
    [Preserve(AllMembers = true)]
	public class MvxAndroidFusedLocationWatcher
		: MvxLocationWatcher
	{
		private Context _context;
        private Context Context => _context ?? (_context = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext);
        private FusedLocationHandler _locationHandler;

		protected override void PlatformSpecificStart(MvxLocationOptions options)
		{
			if (_locationHandler == null)
				_locationHandler = new FusedLocationHandler(this, Context);

			_locationHandler.Start(options);
		}

        protected override void PlatformSpecificStop() => _locationHandler.Stop();

        public override MvxGeoLocation CurrentLocation 
		{
			get 
			{
				if (_locationHandler == null)
					throw new MvxException("Location Manager not started");

				var androidLocation = _locationHandler.GetLastKnownLocation();
				if (androidLocation == null)
					return null;

				return CreateLocation(androidLocation);
			}
		}

		internal void OnLocationUpdated(Android.Locations.Location androidLocation)
		{
			if (androidLocation == null)
			{
				MvxLog.Instance.Trace("Android: Null location seen");
				return;
			}

			if (androidLocation.Latitude == double.MaxValue || 
                androidLocation.Longitude == double.MaxValue)
			{
				MvxLog.Instance.Trace("Android: Invalid location seen");
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
				MvxLog.Instance.Trace("Android: Exception seen in converting location " + exception.ToLongString());
				return;
			}

			SendLocation (location);
		}

        internal void OnLocationError(MvxLocationErrorCode errorCode) => SendError(errorCode);

        internal void OnLocationAvailabilityChanged(bool isAvailable) => 
            Permission = isAvailable ?
                MvxLocationPermission.Granted :
                MvxLocationPermission.Denied;

        private static MvxGeoLocation CreateLocation(Android.Locations.Location androidLocation)
		{
			var position = new MvxGeoLocation { Timestamp = androidLocation.Time.FromMillisecondsUnixTimeToUtc() };
			var coords = position.Coordinates;

            coords.Latitude = androidLocation.Latitude;
            coords.Longitude = androidLocation.Longitude;

            if (androidLocation.HasAltitude)
				coords.Altitude = androidLocation.Altitude;
			if (androidLocation.HasBearing)
				coords.Heading = androidLocation.Bearing;
			if (androidLocation.HasSpeed)
				coords.Speed = androidLocation.Speed;
			if (androidLocation.HasAccuracy)
				coords.Accuracy = androidLocation.Accuracy;

			return position;
		}
	}
}