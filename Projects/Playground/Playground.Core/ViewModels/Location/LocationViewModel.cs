using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Location;
using MvvmCross.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace Playground.Core.ViewModels.Location
{
    public class LocationViewModel : MvxNavigationViewModel
    {
        private readonly IMvxLocationWatcher _locationWatcher;

        public LocationViewModel(
            IMvxLogProvider logProvider,
            IMvxNavigationService navigation,
            IMvxLocationWatcher locationWatcher)
            : base(logProvider, navigation)
        {
            _locationWatcher = locationWatcher;

            StartCommand = new MvxAsyncCommand(DoStartCommand, () => !_locationWatcher.Started);
        }

        public MvxAsyncCommand StartCommand { get; }

        private MvxGeoLocation _lastLocation;
        public MvxGeoLocation LastLocation
        {
            get => _lastLocation;
            set => SetProperty(ref _lastLocation, value);
        }

        private async Task DoStartCommand()
        {
            var status = await RequestPermission();
            if (!status)
                return;

            var options = new MvxLocationOptions
            {
                Accuracy = MvxLocationAccuracy.Coarse,
                TrackingMode = MvxLocationTrackingMode.Foreground,
                TimeBetweenUpdates = TimeSpan.FromSeconds(2)
            };

            _locationWatcher.Start(options, OnLocationUpdated, OnLocationUpdateError);
        }

        private void OnLocationUpdateError(MvxLocationError obj)
        {
            System.Diagnostics.Debug.WriteLine($"Location Error: {obj.Code} {obj.ToString()}");
        }

        private void OnLocationUpdated(MvxGeoLocation obj)
        {
            LastLocation = obj;
        }

        private async Task<bool> RequestPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    var result = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = result[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Permission Error: " + ex);
            }

            return false;
        }
    }
}
