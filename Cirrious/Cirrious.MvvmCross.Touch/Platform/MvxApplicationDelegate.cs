// MvxApplicationDelegate.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Platform;
using UIKit;

namespace Cirrious.MvvmCross.Touch.Platform
{
    public class MvxApplicationDelegate : UIApplicationDelegate
                                          , IMvxLifetime
    {
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

        public override void FinishedLaunching(UIApplication application)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
        }

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            if (handler != null)
                handler(this, new MvxLifetimeEventArgs(which));
        }

        #region IMvxLifetime implementation

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        #endregion
    }
}