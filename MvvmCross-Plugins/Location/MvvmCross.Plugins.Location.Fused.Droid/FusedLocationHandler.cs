using System;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Plugins.Location.Fused.Droid
{
    [Preserve(AllMembers = true)]
	public class FusedLocationHandler
		: LocationCallback
		, GoogleApiClient.IConnectionCallbacks
		, GoogleApiClient.IOnConnectionFailedListener
	{
		private GoogleApiClient _client;
		private LocationRequest _request;

		private readonly MvxAndroidFusedLocationWatcher _owner;
        private readonly Context _context;

		public FusedLocationHandler(MvxAndroidFusedLocationWatcher owner, Context context)
		{
			_owner = owner;
            _context = context;
		}

		public void Start(MvxLocationOptions options)
		{
            if (_client == null)
            {
                EnsureGooglePlayServiceAvailable(_context);
                Initialize(_context);
            }

            if (_client.IsConnected || _client.IsConnecting)
                throw new MvxException("Start has already been called. Please call Stop and try again");
            
            _request = CreateLocationRequest(options);
			_client.Connect();
		}

		public void Stop()
		{
			if (_client == null)
				return;

            if (_client.IsConnected)
                LocationServices.FusedLocationApi.RemoveLocationUpdates(_client, this);

            if (_client.IsConnected || _client.IsConnecting)
            {
                _client.Disconnect();
                _client = null;
            }
		}

		public Android.Locations.Location GetLastKnownLocation()
		{
			if (_client.IsConnected)
				return LocationServices.FusedLocationApi.GetLastLocation(_client);
			return null;
		}

        public void OnConnected(Bundle connectionHint)
        {
            LocationServices.FusedLocationApi.RequestLocationUpdates(_client, _request, this, Looper.MainLooper);

            var location = LocationServices.FusedLocationApi.GetLastLocation(_client);
            if (location != null)
                _owner.OnLocationUpdated(location);

            Mvx.TaggedTrace(MvxTraceLevel.Diagnostic, "Plugin.Location.Fused", "OnConnected");
        }

		public void OnConnectionSuspended(int cause)
		{
            // disconnected
            Mvx.TaggedTrace(MvxTraceLevel.Diagnostic, "Plugin.Location.Fused", "OnConnectionSuspended: {0}", cause);
		}

		public void OnConnectionFailed(ConnectionResult result)
		{
			_owner.OnLocationError(ToMvxLocationErrorCode(result));
            Mvx.TaggedTrace(MvxTraceLevel.Diagnostic, "Plugin.Location.Fused", "OnConnectionFailed: {0}", result);

            _client?.Reconnect();
		}

		public override void OnLocationResult(LocationResult result) =>
			_owner.OnLocationUpdated(result.LastLocation);

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

		private void Initialize(Context context)
		{
			_client = new GoogleApiClient.Builder(context)
				.AddApi(LocationServices.API)
				.AddConnectionCallbacks(this)
				.AddOnConnectionFailedListener(this)
				.Build();
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

		private static MvxLocationErrorCode ToMvxLocationErrorCode(ConnectionResult connectionResult)
		{
			var errorCode = connectionResult.ErrorCode;
            var mvxErrorCode = MvxLocationErrorCode.ServiceUnavailable;

            if (errorCode == ConnectionResult.Timeout)
                mvxErrorCode = MvxLocationErrorCode.Timeout;
            else if (errorCode == ConnectionResult.NetworkError)
                mvxErrorCode = MvxLocationErrorCode.Network;
            else if (errorCode == ConnectionResult.Canceled)
                mvxErrorCode = MvxLocationErrorCode.Canceled;

            // TODO handle more error-codes?
            return mvxErrorCode;
        }
	}
}