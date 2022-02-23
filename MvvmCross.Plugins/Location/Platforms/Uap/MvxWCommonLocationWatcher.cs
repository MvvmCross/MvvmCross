// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Exceptions;
using Windows.Devices.Geolocation;

namespace MvvmCross.Plugin.Location.Platforms.Uap
{
    public sealed class MvxWCommonLocationWatcher : MvxLocationWatcher
    {
        private Geolocator _geolocator;

        public MvxWCommonLocationWatcher()
        {
            EnsureStopped();
        }

        protected override void PlatformSpecificStart(MvxLocationOptions options)
        {
            if (_geolocator != null)
                throw new MvxException("You cannot start the MvxLocation service more than once");

            _geolocator = new Geolocator
            {
                DesiredAccuracy = options.Accuracy == MvxLocationAccuracy.Fine ? PositionAccuracy.High : PositionAccuracy.Default,
                MovementThreshold = options.MovementThresholdInM,
                ReportInterval = (uint)options.TimeBetweenUpdates.TotalMilliseconds
            };

            _geolocator.StatusChanged += OnStatusChanged;
            _geolocator.PositionChanged += OnPositionChanged;
        }

        public override MvxGeoLocation CurrentLocation
        {
            get
            {
                if (_geolocator == null)
                    throw new MvxException("Location Manager not started");

#warning Add async API for GeoLocation.
                var storeLocation = _geolocator.GetGeopositionAsync().AsTask().GetAwaiter().GetResult();
                if (storeLocation == null)
                    return null;

                return CreateLocation(storeLocation.Coordinate);
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
                // TODO - is this enough to stop it?
                _geolocator.PositionChanged -= OnPositionChanged;
                _geolocator.StatusChanged -= OnStatusChanged;
                _geolocator = null;
            }
        }

        private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            var location = CreateLocation(args.Position.Coordinate);
            SendLocation(location);
        }

        private void OnStatusChanged(Geolocator geolocator, StatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case PositionStatus.Ready:
                case PositionStatus.Initializing:
                    Permission = MvxLocationPermission.Granted;
                    break;

                case PositionStatus.NoData:
                    // TODO Permission = Unknown? Denied?
                    // TODO - trace could be useful here?
                    SendError(MvxLocationErrorCode.PositionUnavailable);
                    break;

                case PositionStatus.Disabled:
                case PositionStatus.NotInitialized:
                case PositionStatus.NotAvailable:
                    Permission = MvxLocationPermission.Denied;
                    // TODO - trace could be useful here?
                    SendError(MvxLocationErrorCode.ServiceUnavailable);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private MvxGeoLocation CreateLocation(Geocoordinate coordinate)
        {
            var position = new MvxGeoLocation { Timestamp = coordinate.Timestamp };
            var coords = position.Coordinates;

            coords.Altitude = coordinate.Point.Position.Altitude;
            coords.Latitude = coordinate.Point.Position.Latitude;
            coords.Longitude = coordinate.Point.Position.Longitude;
            coords.Speed = coordinate.Speed ?? 0.0;
            coords.Accuracy = coordinate.Accuracy;
            coords.AltitudeAccuracy = coordinate.AltitudeAccuracy ?? double.MaxValue;

            return position;
        }
    }
}
