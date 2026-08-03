// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using MvvmCross.Core;

namespace MvvmCross.Platforms.Mac.Core
{
    /// <summary>
    /// Base application delegate that fires MvvmCross lifetime events.
    /// Use <see cref="MvvmCross.Platforms.Mac.Hosting.MvxMacHostBuilder"/> in your
    /// <see cref="DidFinishLaunching"/> override to initialize the framework.
    /// </summary>
    public abstract class MvxApplicationDelegate : NSApplicationDelegate, IMvxApplicationDelegate
    {
        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        public override void WillBecomeActive(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }

        public override void DidResignActive(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }

        public override void WillTerminate(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
        }

        protected void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
        }
    }
}

