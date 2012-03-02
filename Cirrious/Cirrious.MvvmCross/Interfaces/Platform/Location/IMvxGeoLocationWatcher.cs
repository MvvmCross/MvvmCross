using System;

namespace Cirrious.MvvmCross.Interfaces.Platform.Location
{
    public interface IMvxGeoLocationWatcher : IDisposable
    {
        void Start(MvxGeoLocationOptions options, Action<MvxGeoLocation> success, Action<MvxLocationError> error);
        void Stop();
    }
}