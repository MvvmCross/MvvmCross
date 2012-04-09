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
#warning Credit to ChrisNTR fro some of this code

using System;
using System.Device.Location;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Platform.Location;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Platform.Location;

namespace Cirrious.MvvmCross.WindowsPhone.Platform.Location
{
    public sealed class MvxWindowsPhoneGeoLocationWatcher : MvxBaseGeoLocationWatcher
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
#warning _geoWatcher.MovementThreshold needed too
            // _geoWatcher.MovementThreshold
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
                case GeoPositionStatus.Ready:
#warning is it ok to not pass this on?
                    /*
                    var position = CreateLocation(_geoWatcher.Position.Location);
                    position.Timestamp = _geoWatcher.Position.Timestamp;
                    _currentPositionCallback(position);
                     */
                    break;
                default:
#warning is it ok to ignore other codes?
                    break;
            }
        }

        private static MvxGeoLocation CreateLocation(GeoCoordinate coordinate, DateTimeOffset timestamp)
        {
            var position = new MvxGeoLocation {Timestamp = timestamp};
            var coords = position.Coordinates;

            coords.Altitude = coordinate.Altitude;
            coords.Latitude = coordinate.Latitude;
            coords.Longitude = coordinate.Longitude;
            coords.Speed = coordinate.Speed;
            coords.Accuracy = coordinate.HorizontalAccuracy;
            coords.AltitudeAccuracy = coordinate.VerticalAccuracy;

            return position;
        }
    }
}
