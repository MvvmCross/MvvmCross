using System;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.Interfaces.Platform.Location;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ViewModels;

namespace Tutorial.Core.ViewModels.Lessons
{
    public class LocationViewModel
        : MvxViewModel
        , IMvxServiceConsumer<IMvxGeoLocationWatcher>
    {
        private readonly IMvxGeoLocationWatcher _watcher;

        public LocationViewModel()
        {
            _watcher = this.GetService<IMvxGeoLocationWatcher>();
            ViewUnRegistered += OnViewUnRegistered;
        }

        private void OnViewUnRegistered(object sender, EventArgs eventArgs)
        {
            if (IsStarted)
            {
                DoStartStop();
            }
        }

        private bool _isStarted;
        public bool IsStarted
        {
            get { return _isStarted; }
            set { _isStarted = value; FirePropertyChanged(() => IsStarted); }
        }

        private double _lat;
        public double Lat
        {
            get { return _lat; }
            set { _lat = value; FirePropertyChanged(() => Lat); }
        }

        private double _lng;
        public double Lng
        {
            get { return _lng; }
            set { _lng = value; FirePropertyChanged(() => Lng); }
        }

        private string _lastError;
        public string LastError
        {
            get { return _lastError; }
            set { _lastError = value; FirePropertyChanged(() => LastError); }
        }

        public IMvxCommand StartStopCommand
        {
            get
            {
                return new MvxRelayCommand(DoStartStop);
            }
        }

        private void DoStartStop()
        {
            if (!IsStarted)
            {
                _watcher.Start(new MvxGeoLocationOptions() { EnableHighAccuracy = true }, OnNewLocation, OnError);
            }
            else
            {
                _watcher.Stop();
            }

            IsStarted = !IsStarted;
        }

        private void OnError(MvxLocationError error)
        {
            // TODO - shuold handle the error better than this really!
            LastError = error.Code.ToString();
        }

        private void OnNewLocation(MvxGeoLocation location)
        {
            if (location != null && location.Coordinates != null)
            {
                Lat = location.Coordinates.Latitude;
                Lng = location.Coordinates.Longitude;
            }
        }
    }
}