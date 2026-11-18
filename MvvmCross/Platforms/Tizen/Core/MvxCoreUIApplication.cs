// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Core;
using Tizen.Applications;

namespace MvvmCross.Platforms.Tizen.Core
{
    /// <summary>
    /// Base Tizen application class that fires MvvmCross lifetime events.
    /// Initialize MvvmCross via a host builder in your <see cref="OnCreate"/> override.
    /// </summary>
    public abstract class MvxCoreUIApplication : CoreUIApplication, IMvxLifetime
    {
        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        protected override void OnResume()
        {
            base.OnResume();
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }

        protected override void OnPause()
        {
            base.OnPause();
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }

        protected override void OnTerminate()
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
            base.OnTerminate();
        }

        protected void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
        }
    }
}

