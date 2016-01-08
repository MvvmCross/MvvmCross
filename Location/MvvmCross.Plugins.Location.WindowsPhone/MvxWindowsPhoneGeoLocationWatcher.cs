// MvxWindowsPhoneGeoLocationWatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Exceptions;
using System;
using System.Device.Location;

namespace MvvmCross.Plugins.Location.WindowsPhone
{
    [Obsolete("Use MvxWindowsPhoneLocationWatcher instead")]
    public sealed class MvxWindowsPhoneGeoLocationWatcher : MvxGeoLocationWatcher
    {
        private GeoCoordinateWatcher _geoWatcher;

        public MvxWindowsPhoneGeoLocationWatcher()
        {
            EnsureStopped();
        }

        protected override void PlatformSpecificStart(MvxGeoLocationOptions options)
        {
            if (_geoWatcher != null)
                throw new MvxException("You cannot start the MvxLocation service more than once");

            _geoWatcher =
                new GeoCoordinateWatcher(options.EnableHighAccuracy
                                             ? GeoPositionAccuracy.High
                                             : GeoPositionAccuracy.Default);

            // see https://github.com/slodge/MvvmCross/issues/90 re: _geoWatcher.MovementThreshold
            _geoWatcher.StatusChanged += OnStatusChanged;
            _geoWatcher.PositionChanged += OnPositionChanged;
            _geoWatcher.Start();
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
                    var errorCode = _geoWatcher.Permission == GeoPositionPermission.Denied
                                        ? MvxLocationErrorCode.PermissionDenied
                                        : MvxLocationErrorCode.PositionUnavailable;
                    SendError(errorCode);
                    break;

                case GeoPositionStatus.Initializing:
                case GeoPositionStatus.Ready:
                    // not an error - so ignored
                    break;

                default:
                    // other codes ignored
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

#warning Heading needed?

            return position;
        }
    }
}