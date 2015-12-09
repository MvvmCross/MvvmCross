// <copyright file="MvxApplicationDelegate.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com


#if __UNIFIED__
using AppKit;
#else
#endif

namespace MvvmCross.Mac.Platform
{
    using System;

    using global::MvvmCross.Core.Platform;

    public class MvxApplicationDelegate : NSApplicationDelegate
                                          , IMvxLifetime
    {
#warning Removed this lifetime functionality....
        /*
        public override void WillEnterForeground (NSApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }

        public override void DidEnterBackground (NSApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }

        public override void WillTerminate (NSApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
        }

        public override void FinishedLaunching (NSApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
        }
		*/

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = this.LifetimeChanged;
            if (handler != null)
                handler(this, new MvxLifetimeEventArgs(which));
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
    }
}