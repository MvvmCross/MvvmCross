// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base.Core;

namespace MvvmCross.Plugin.Location
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

        event EventHandler<MvxValueEventArgs<MvxLocationPermission>> OnPermissionChanged;
    }
}
