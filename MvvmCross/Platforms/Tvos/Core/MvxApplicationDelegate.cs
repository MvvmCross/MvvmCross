// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Core;

namespace MvvmCross.Platforms.Tvos.Core
{
    /// <summary>
    /// Base application delegate that fires MvvmCross lifetime events.
    /// Use <see cref="MvvmCross.Platforms.Tvos.Hosting.MvxTvosHostBuilder"/> in your
    /// <see cref="FinishedLaunching"/> override to initialize the framework.
    /// </summary>
    public abstract class MvxApplicationDelegate : UIApplicationDelegate, IMvxApplicationDelegate
    {
        /// <summary>
        /// UIApplicationDelegate.Window doesn't really exist / work. It was added by Xamarin.iOS templates.
        /// </summary>
        public virtual UIWindow MainWindow { get; set; }

        public override void WillEnterForeground(UIApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }

        public override void DidEnterBackground(UIApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }

        public override void WillTerminate(UIApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        protected void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
        }
    }
}

