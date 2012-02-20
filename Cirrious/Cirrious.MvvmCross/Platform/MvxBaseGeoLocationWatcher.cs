using System;
using Cirrious.MvvmCross.Interfaces.Services.Location;

namespace Cirrious.MvvmCross.Platform
{
    public abstract class MvxBaseGeoLocationWatcher 
#if MonoDroid
         : Java.Lang.Object
        , IMvxGeoLocationWatcher
#else
        : IMvxGeoLocationWatcher
#endif
    {
        private Action<MvxGeoLocation> _locationCallback;
        private Action<MvxLocationError> _errorCallback;

        ~MvxBaseGeoLocationWatcher()
        {
            Dispose(false);    
        }

#if MonoDroid
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (!isDisposing)
                return;

            Stop();
        }
#else
        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool isDisposing)
        {
            if (!isDisposing)
                return;

            Stop();
        }
#endif

        public void Start(MvxGeoLocationOptions options, Action<MvxGeoLocation> success, Action<MvxLocationError> error)
        {
            lock (this)
            {
                _locationCallback = success;
                _errorCallback = error;

                PlatformSpecificStart(options);
            }
        }

        public void Stop()
        {
            lock (this)
            {
                _locationCallback = position => { };
                _errorCallback = error => { };

                PlatformSpecificStop();
            }
        }

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