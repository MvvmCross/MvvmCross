// MvxWindowsPhoneLocationWatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Device.Location;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.MvvmCross.Plugins.Location.WindowsPhone
{
    public sealed class MvxWindowsPhoneLocationWatcher : MvxLocationWatcher
    {
        private GeoCoordinateWatcher _geoWatcher;

        public MvxWindowsPhoneLocationWatcher()
        {
            EnsureStopped();
        }

        protected override void PlatformSpecificStart(MvxLocationOptions options)
        {
            if (_geoWatcher != null)
                throw new MvxException("You cannot start the MvxLocation service more than once");

            _geoWatcher =
                new GeoCoordinateWatcher(options.Accuracy == MvxLocationAccuracy.Fine
                                             ? GeoPositionAccuracy.High
                                             : GeoPositionAccuracy.Default);

            _geoWatcher.MovementThreshold = options.MovementThresholdInM;
            if (options.TimeBetweenUpdates > TimeSpan.Zero)
            {
                Mvx.Warning("TimeBetweenUpdates specified for MvxLocationOptions - but this is not supported in WindowsPhone");
            }

            _geoWatcher.StatusChanged += OnStatusChanged;
            _geoWatcher.PositionChanged += OnPositionChanged;
            _geoWatcher.Start();
        }

        public override MvxGeoLocation CurrentLocation
        {
            get
            {
                if (_geoWatcher == null)
                    throw new MvxException("Location Manager not started");

                var phoneLocation = _geoWatcher.Position;
                if (phoneLocation == null)
                    return null;

                return CreateLocation(phoneLocation.Location, phoneLocation.Timestamp);
            }
        }

        protected override void PlatformSpecificStop()
        {
            EnsureStopped();
        }

        private void EnsureStopped()
        {
            if (_geoWatcher != null)
            {
                _geoWatcher.Stop();
                _geoWatcher.StatusChanged -= OnStatusChanged;
                _geoWatcher.PositionChanged -= OnPositionChanged;
                _geoWatcher = null;
            }
        }

        private void OnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var location = CreateLocation(e.Position.Location, e.Position.Timestamp);
            SendLocation(location);
        }

        private void OnStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.NoData:
                case GeoPositionStatus.Disabled:
					Permission = MvxLocationPermission.Denied;
                    var errorCode = _geoWatcher.Permission == GeoPositionPermission.Denied
                                        ? MvxLocationErrorCode.PermissionDenied
                                        : MvxLocationErrorCode.PositionUnavailable;
                    SendError(errorCode);
                    break;
                case GeoPositionStatus.Initializing:
                case GeoPositionStatus.Ready:
					Permission = MvxLocationPermission.Granted;
                    // not an error - so ignored
                    break;
                default:
                    // other codes ignored
					// TODO do other codes affect Permission?
                    break;
            }
        }

        private static MvxGeoLocation CreateLocation(GeoCoordinate coordinate, DateTimeOffset timestamp)
        {
            var position = new MvxGeoLocation { Timestamp = timestamp };
            var coords = position.Coordinates;

            coords.Altitude = coordinate.Altitude;
            coords.Latitude = coordinate.Latitude;
            coords.Longitude = coordinate.Longitude;
            coords.Speed = coordinate.Speed;
            coords.Accuracy = coordinate.HorizontalAccuracy;
            coords.AltitudeAccuracy = coordinate.VerticalAccuracy;
            coords.Heading = coordinate.Course;

            return position;
        }
    }
}