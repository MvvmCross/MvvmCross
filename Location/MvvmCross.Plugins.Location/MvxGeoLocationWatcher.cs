// MvxGeoLocationWatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Plugins.Location
{
    [Obsolete("Use MvxLocationWatcher instead")]
    public abstract class MvxGeoLocationWatcher
        : IMvxGeoLocationWatcher
    {
        private Action<MvxGeoLocation> _locationCallback;
        private Action<MvxLocationError> _errorCallback;

        public void Start(MvxGeoLocationOptions options, Action<MvxGeoLocation> success, Action<MvxLocationError> error)
        {
            lock (this)
            {
                _locationCallback = success;
                _errorCallback = error;

                PlatformSpecificStart(options);

                Started = true;
            }
        }

        public void Stop()
        {
            lock (this)
            {
                _locationCallback = position => { };
                _errorCallback = error => { };

                PlatformSpecificStop();

                Started = false;
            }
        }

        public bool Started { get; set; }

        protected abstract void PlatformSpecificStart(MvxGeoLocationOptions options);

        protected abstract void PlatformSpecificStop();

        protected virtual void SendLocation(MvxGeoLocation location)
        {
            var callback = _locationCallback;
            if (callback != null)
                callback(location);
        }

        protected void SendError(MvxLocationErrorCode code)
        {
            SendError(new MvxLocationError(code));
        }

        protected void SendError(MvxLocationError error)
        {
            var errorCallback = _errorCallback;
            if (errorCallback != null)
                errorCallback(error);
        }
    }
}