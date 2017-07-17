using System;
using AppKit;
using MvvmCross.Core.Platform;

namespace MvvmCross.Mac.Platform
{
    public class MvxApplicationDelegate : NSApplicationDelegate, IMvxLifetime
    {
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

        public override void DidFinishLaunching(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
        }

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            if (handler != null)
                handler(this, new MvxLifetimeEventArgs(which));
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
    }
}