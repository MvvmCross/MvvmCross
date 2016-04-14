using System;
using Android.OS;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Exceptions;

namespace MvvmCross.Plugins.Location.Fused.Droid
{
	public class FusedLocationHandler
		: LocationCallback
		, GoogleApiClient.IConnectionCallbacks
		, GoogleApiClient.IOnConnectionFailedListener
	{
		private GoogleApiClient _client;
		private LocationRequest _request;

		private readonly MvxAndroidFusedLocationWatcher _owner;

		public FusedLocationHandler (MvxAndroidFusedLocationWatcher owner, Context context)
		{
			_owner = owner;
			EnsureGooglePlayServiceAvailable (context);
			Initialize (context);
		}

		public void Start (MvxLocationOptions options)
		{
			_request = CreateLocationRequest (options);
			_client.Connect ();
		}

		public void Stop ()
		{
			if (_client == null) {
				return;
			}

			LocationServices.FusedLocationApi.RemoveLocationUpdates(_client, this);

			_client.Disconnect ();
		}

		public Android.Locations.Location GetLastKnownLocation ()
		{
			if (_client.IsConnected)
			{
				return LocationServices.FusedLocationApi.GetLastLocation(_client);
			}
			return null;
		}

		public void OnConnected (Bundle connectionHint)
		{
			LocationServices.FusedLocationApi.RequestLocationUpdates (_client, _request, this, Looper.MainLooper);

			var location = LocationServices.FusedLocationApi.GetLastLocation(_client);
			if (location != null)
			{
				_owner.OnLocationUpdated (location);
			}
		}

		public void OnConnectionSuspended (int cause)
		{
			// disconnected
			MvxTrace.Trace ("Plugin.Location.Fused.OnConnectionSuspended: " + cause);
		}

		public void OnConnectionFailed (ConnectionResult result)
		{
			_owner.OnLocationError (ToMvxLocationErrorCode (result));
			MvxTrace.Trace ("Plugin.Location.Fused.OnConnectionFailed: " + result);
		}

		public override void OnLocationResult (LocationResult result)
		{
			_owner.OnLocationUpdated (result.LastLocation);
		}

		public override void OnLocationAvailability (LocationAvailability locationAvailability)
		{
			var available = locationAvailability != null && locationAvailability.IsLocationAvailable;
			_owner.OnLocationAvailabilityChanged (available);
		}

		private void EnsureGooglePlayServiceAvailable (Context context)
		{
			var availability = GoogleApiAvailability.Instance;
			var result = availability.IsGooglePlayServicesAvailable (context);
			if (result != ConnectionResult.Success)
			{
				var errorMessage = "GooglePlayService is not available";
				if (availability.IsUserResolvableError (result))
				{
					errorMessage = availability.GetErrorString (result);
				}
				throw new MvxException (errorMessage);
			}
		}

		private void Initialize (Context context)
		{
			_client = new GoogleApiClient.Builder (context)
				.AddApi (LocationServices.API)
				.AddConnectionCallbacks (this)
				.AddOnConnectionFailedListener (this)
				.Build ();
		}

		private static LocationRequest CreateLocationRequest (MvxLocationOptions options)
		{
			// NOTE options.TrackingMode is not supported

			var request = LocationRequest.Create ();

			switch (options.Accuracy) {
			case MvxLocationAccuracy.Fine:
				request.SetPriority (LocationRequest.PriorityHighAccuracy);
				break;
			case MvxLocationAccuracy.Coarse:
				request.SetPriority (LocationRequest.PriorityBalancedPowerAccuracy);
				break;
			default:
				throw new ArgumentOutOfRangeException ();
			}

			request.SetInterval ((long)options.TimeBetweenUpdates.TotalMilliseconds);
			request.SetSmallestDisplacement (options.MovementThresholdInM);

			return request;
		}

		private static MvxLocationErrorCode ToMvxLocationErrorCode (ConnectionResult connectionResult)
		{
			var errorCode = connectionResult.ErrorCode;

			if (errorCode == ConnectionResult.Timeout) {
				return MvxLocationErrorCode.Timeout;
			}

			// TODO handle more error-codes?

			return MvxLocationErrorCode.ServiceUnavailable;
		}
	}
}

