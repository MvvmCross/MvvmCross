namespace MvvmCross.Mac.Platform
{
    using System;

    using AppKit;

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