﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base;

namespace MvvmCross.Plugin.Location
{
    public abstract class MvxLocationWatcher
        : IMvxLocationWatcher
    {
        private Action<MvxGeoLocation> _locationCallback;
        private Action<MvxLocationError> _errorCallback;

        public event EventHandler<MvxValueEventArgs<MvxLocationPermission>> OnPermissionChanged = delegate { };

        private MvxLocationPermission _permission = MvxLocationPermission.Unknown;

        protected MvxLocationPermission Permission
        {
            get
            {
                return _permission;
            }
            set
            {
                if (_permission != value)
                {
                    _permission = value;
                    OnPermissionChanged(this, new MvxValueEventArgs<MvxLocationPermission>(value));
                }
            }
        }

        public void Start(MvxLocationOptions options, Action<MvxGeoLocation> success, Action<MvxLocationError> error)
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

        public abstract MvxGeoLocation CurrentLocation { get; }
        public MvxGeoLocation LastSeenLocation { get; protected set; }

        protected abstract void PlatformSpecificStart(MvxLocationOptions options);

        protected abstract void PlatformSpecificStop();

        protected virtual void SendLocation(MvxGeoLocation location)
        {
            LastSeenLocation = location;
            var callback = _locationCallback;
            callback?.Invoke(location);
        }

        protected void SendError(MvxLocationErrorCode code)
        {
            SendError(new MvxLocationError(code));
        }

        protected void SendError(MvxLocationError error)
        {
            var errorCallback = _errorCallback;
            errorCallback?.Invoke(error);
        }
    }
}
