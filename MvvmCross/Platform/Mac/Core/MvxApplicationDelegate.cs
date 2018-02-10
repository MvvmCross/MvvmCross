// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using MvvmCross.Core;

namespace MvvmCross.Platform.Mac.Core
{
    public class MvxApplicationDelegate : NSApplicationDelegate, IMvxApplicationDelegate
    {
        public override void DidFinishLaunching(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            base.DidFinishLaunching(notification);
        }

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
            base.WillTerminate(notification);
        }

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
    }
}
