// MvxWpfLocationWatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Exceptions;
using System;
using System.Device.Location;

namespace MvvmCross.Plugins.Location.Wpf
{
    /// <summary>
    /// Location Watcher Plugin for WPF developers.
    /// </summary>
    /// <remarks>Warning - this is not fully tested - see https://github.com/MvvmCross/MvvmCross/pull/632
    /// </remarks>
    /// <see cref="http://code.msdn.microsoft.com/windowsdesktop/Windows-7-Geolocation-API-25585fac"/>
    /// <seealso cref="http://www.techsupportalert.com/content/how-enable-or-disable-location-sensing-windows-7-and-8.htm"/>
    public sealed class MvxWpfLocationWatcher : MvxLocationWatcher
    {
        private GeoCoordinateWatcher _geolocator;

        private MvxGeoLocation _lastKnownPosition = null;

        public MvxWpfLocationWatcher()
        {
            EnsureStopped();
        }

        protected override void PlatformSpecificStart(MvxLocationOptions options)
        {
            if (_geolocator != null)
                throw new MvxException("You cannot start the MvxLocation service more than once");

            _geolocator = new GeoCoordinateWatcher
            {
                MovementThreshold = options.MovementThresholdInM
            };

            _geolocator.TryStart(false, TimeSpan.FromMilliseconds((uint)options.TimeBetweenUpdates.TotalMilliseconds));

            _geolocator.StatusChanged += OnStatusChanged;
            _geolocator.PositionChanged += OnPositionChanged;
        }

        public override MvxGeoLocation CurrentLocation
        {
            get
            {
                if (_geolocator == null)
                {
                    throw new MvxException("Location Manager not started");
                }
                else
                {
                    return _lastKnownPosition;
                }
            }
        }

        protected override void PlatformSpecificStop()
        {
            EnsureStopped();
        }

        private void EnsureStopped()
        {
            if (_geolocator != null)
            {
                _geolocator.Stop();
                _geolocator.PositionChanged -= OnPositionChanged;
                _geolocator.StatusChanged -= OnStatusChanged;
                _geolocator.Dispose();
                _geolocator = null;
            }
        }

        private void OnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
        {
            var location = CreateLocation(args.Position.Location);
            SendLocation(location);
        }

        private void OnStatusChanged(object sender, GeoPositionStatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case GeoPositionStatus.Ready:
                case GeoPositionStatus.Initializing:
                    Permission = MvxLocationPermission.Granted;
                    break;

                case GeoPositionStatus.NoData:
                    // TODO - trace could be useful here?
                    SendError(MvxLocationErrorCode.PositionUnavailable);
                    break;

                case GeoPositionStatus.Disabled:
                    Permission = MvxLocationPermission.Denied;
                    // TODO - trace could be useful here?
                    SendError(MvxLocationErrorCode.ServiceUnavailable);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private MvxGeoLocation CreateLocation(GeoCoordinate coordinate)
        {
            var position = new MvxGeoLocation { Timestamp = DateTime.Now };
            var coords = position.Coordinates;

            // TODO - Not sure how to deal with zero values - as we can't distinguish between null and zero values...
            coords.Altitude = coordinate.Altitude;
            coords.Latitude = coordinate.Latitude;
            coords.Longitude = coordinate.Longitude;
            coords.Speed = coordinate.Speed;
            coords.Accuracy = coordinate.HorizontalAccuracy;
            coords.AltitudeAccuracy = coordinate.VerticalAccuracy;

            _lastKnownPosition = position;

            return position;
        }
    }
}