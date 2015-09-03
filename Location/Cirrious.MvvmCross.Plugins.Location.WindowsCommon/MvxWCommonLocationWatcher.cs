// MvxStoreLocationWatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Windows.Devices.Geolocation;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.WindowsCommon.Platform;

namespace MvvmCross.Plugins.Location.WindowsCommon
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

#warning This Await here feels very dangerous - would be better to add an async API for location
                var storeLocation = _geolocator.GetGeopositionAsync().Await();
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

            // TODO - allow nullables - https://github.com/slodge/MvvmCross/issues/94
            coords.Altitude = coordinate.Altitude ?? 0.0;
            coords.Latitude = coordinate.Latitude;
            coords.Longitude = coordinate.Longitude;
            coords.Speed = coordinate.Speed ?? 0.0;
            coords.Accuracy = coordinate.Accuracy;
            coords.AltitudeAccuracy = coordinate.AltitudeAccuracy ?? double.MaxValue;

            return position;
        }
    }
}