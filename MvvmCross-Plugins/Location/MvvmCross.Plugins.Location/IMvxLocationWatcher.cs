// IMvxGeoLocationWatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform.Core;

namespace MvvmCross.Plugins.Location
{
    public interface IMvxLocationWatcher
    {
        bool Started { get; }
        MvxGeoLocation CurrentLocation { get; }
        MvxGeoLocation LastSeenLocation { get; }

        void Start(
            MvxLocationOptions options,
            Action<MvxGeoLocation> success,
            Action<MvxLocationError> error);

        void Stop();

        event EventHandler<MvxValueEventArgs<MvxLocationPermission>> OnPermissionChanged;
    }
}