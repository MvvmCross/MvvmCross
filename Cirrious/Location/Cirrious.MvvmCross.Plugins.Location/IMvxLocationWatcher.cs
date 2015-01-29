// IMvxGeoLocationWatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Plugins.Location
{
    public interface IMvxLocationWatcher
    {
        void Start(
            MvxLocationOptions options,
            Action<MvxGeoLocation> success,
            Action<MvxLocationError> error);
        void Stop();
        bool Started { get; }
        MvxGeoLocation CurrentLocation { get; }
        MvxGeoLocation LastSeenLocation { get; }
		event Action<MvxLocationPermission> OnPermissionChanged;
    }
}