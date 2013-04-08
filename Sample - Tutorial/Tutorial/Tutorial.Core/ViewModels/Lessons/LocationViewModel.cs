using System;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Location;
using Cirrious.MvvmCross.ViewModels;

namespace Tutorial.Core.ViewModels.Lessons
{
    public class LocationViewModel
        : MvxViewModel        
    {
        private readonly IMvxGeoLocationWatcher _watcher;

        public LocationViewModel()
        {
            PluginLoader.Instance.EnsureLoaded();
            _watcher = Mvx.Resolve<IMvxGeoLocationWatcher>();
#warning need IsStarted check here...
        }

        private bool _isStarted;
        public bool IsStarted
        {
            get { return _isStarted; }
            set { _isStarted = value; RaisePropertyChanged(() => IsStarted); }
        }

        private double _lat;
        public double Lat
        {
            get { return _lat; }
            set { _lat = value; RaisePropertyChanged(() => Lat); }
        }

        private double _lng;
        public double Lng
        {
            get { return _lng; }
            set { _lng = value; RaisePropertyChanged(() => Lng); }
        }

        private string _lastError;
        public string LastError
        {
            get { return _lastError; }
            set { _lastError = value; RaisePropertyChanged(() => LastError); }
        }

        public ICommand StartStopCommand
        {
            get
            {
                return new MvxCommand(DoStartStop);
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