// MvxApplicationDelegate.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Touch.Platform
{
    using System;

    using MvvmCross.Core.Platform;

    using UIKit;

    public class MvxApplicationDelegate : UIApplicationDelegate
                                          , IMvxApplicationDelegate
    {
        public override void WillEnterForeground(UIApplication application)
        {
            this.FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }

        public override void DidEnterBackground(UIApplication application)
        {
            this.FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }

        public override void WillTerminate(UIApplication application)
        {
            this.FireLifetimeChanged(MvxLifetimeEvent.Closing);
        }

        public override void FinishedLaunching(UIApplication application)
        {
            this.FireLifetimeChanged(MvxLifetimeEvent.Launching);
        }

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = this.LifetimeChanged;
            handler?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        #region IMvxLifetime implementation

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        #endregion IMvxLifetime implementation
    }
}