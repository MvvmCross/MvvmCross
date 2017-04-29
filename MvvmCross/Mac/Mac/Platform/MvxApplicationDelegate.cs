using System;
using AppKit;
using MvvmCross.Core.Platform;

namespace MvvmCross.Mac.Platform
{
    public class MvxApplicationDelegate : NSApplicationDelegate
        , IMvxLifetime
    {
        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
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
            var handler = LifetimeChanged;
            if (handler != null)
                handler(this, new MvxLifetimeEventArgs(which));
        }
    }
}