// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.OS;
using MvvmCross.Exceptions;

namespace MvvmCross.Plugin.Location.Fused
{
    [Preserve(AllMembers = true)]
    public class FusedLocationHandler
        : LocationCallback
    {
        private FusedLocationProviderClient _client;
        private LocationRequest _request;
        private readonly MvxAndroidFusedLocationWatcher _owner;
        private readonly Context _context;

        public global::Android.Locations.Location LastKnownLocation { get; private set; }

        public FusedLocationHandler(MvxAndroidFusedLocationWatcher owner, Context context)
        {
            _owner = owner;
            _context = context;
        }

        public async Task StartAsync(MvxLocationOptions options)
        {
            EnsureGooglePlayServiceAvailable(_context);

            if (_client == null)
                _client = LocationServices.GetFusedLocationProviderClient(_context);

            // Create location request.
            _request = CreateLocationRequest(options);

            // Start receiving location updates.
            await _client.RequestLocationUpdatesAsync(_request, this, Looper.MainLooper);
        }

        public async Task StopAsync()
        {
            if (_client != null)
                await _client.RemoveLocationUpdatesAsync(this);
        }

        public override void OnLocationResult(LocationResult result)
        {
            LastKnownLocation = result.LastLocation;
            _owner.OnLocationUpdated(result.LastLocation);
        }

        public override void OnLocationAvailability(LocationAvailability locationAvailability)
        {
            var available = locationAvailability != null && locationAvailability.IsLocationAvailable;
            _owner.OnLocationAvailabilityChanged(available);
        }

        private void EnsureGooglePlayServiceAvailable(Context context)
        {
            var availability = GoogleApiAvailability.Instance;
            var result = availability.IsGooglePlayServicesAvailable(context);
            if (result != ConnectionResult.Success)
            {
                var errorMessage = "GooglePlayService is not available";
                if (availability.IsUserResolvableError(result))
                    errorMessage = availability.GetErrorString(result);
                throw new MvxException(errorMessage);
            }
        }

        private static LocationRequest CreateLocationRequest(MvxLocationOptions options)
        {
            // NOTE options.TrackingMode is not supported
            var request = LocationRequest.Create();

            switch (options.Accuracy)
            {
                case MvxLocationAccuracy.Fine:
                    request.SetPriority(LocationRequest.PriorityHighAccuracy);
                    break;
                case MvxLocationAccuracy.Coarse:
                    request.SetPriority(LocationRequest.PriorityBalancedPowerAccuracy);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            request.SetInterval((long)options.TimeBetweenUpdates.TotalMilliseconds);
            request.SetSmallestDisplacement(options.MovementThresholdInM);

            return request;
        }
    }
}
