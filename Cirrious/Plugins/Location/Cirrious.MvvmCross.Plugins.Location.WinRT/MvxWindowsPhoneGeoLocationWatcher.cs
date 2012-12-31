#region Copyright

// <copyright file="MvxWindowsPhoneGeoLocationWatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Cirrious.MvvmCross.Exceptions;
using Windows.Devices.Geolocation;

namespace Cirrious.MvvmCross.Plugins.Location.WinRT
{
    public sealed class MvxWinRTGeoLocationWatcher : MvxBaseGeoLocationWatcher
    {
        private Windows.Devices.Geolocation.Geolocator _geolocator;

        public MvxWinRTGeoLocationWatcher()
        {
            EnsureStopped();
        }

        protected override void PlatformSpecificStart(MvxGeoLocationOptions options)
        {
            if (_geolocator != null)
                throw new MvxException("You cannot start the MvxLocation service more than once");

            // see https://github.com/slodge/MvvmCross/issues/90
            _geolocator = new Geolocator
                {
                    // DesiredAccuracy = TODO options.EnableHighAccuracy
                    // MovementThreshold = TODO
                    // ReportInterval = TODO
                };

            _geolocator.StatusChanged += OnStatusChanged;
            _geolocator.PositionChanged += OnPositionChanged;
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
                    break;
                case PositionStatus.Initializing:
                    break;
                case PositionStatus.NoData:
                    // TODO - trace could be useful here?
                    SendError(MvxLocationErrorCode.PositionUnavailable);
                    break;
                case PositionStatus.Disabled:
                    // TODO - trace could be useful here?
                    SendError(MvxLocationErrorCode.ServiceUnavailable);
                    break;
                case PositionStatus.NotInitialized:
                    // TODO - trace could be useful here?
                    SendError(MvxLocationErrorCode.ServiceUnavailable);
                    break;
                case PositionStatus.NotAvailable:
                    // TODO - trace could be useful here?
                    SendError(MvxLocationErrorCode.ServiceUnavailable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private MvxGeoLocation CreateLocation(Geocoordinate coordinate)
        {
            var position = new MvxGeoLocation {Timestamp = coordinate.Timestamp};
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