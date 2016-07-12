// MvxFormsIosPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Martin Nygren, @zzcgumn, zzcgumn@me.com

using System;
using MvvmCross.iOS.Platform;
using MvvmCross.Core.Platform;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace MvvmCross.Forms.Views.iOS
{
    public class MvxFormsApplicationDelegate : FormsApplicationDelegate, IMvxApplicationDelegate, IMvxLifetime
    {
        public override bool FinishedLaunching(UIApplication uiApplication, Foundation.NSDictionary launchOptions)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
            base.WillEnterForeground(uiApplication);
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
            base.DidEnterBackground(uiApplication);
        }

        public override void WillTerminate(UIApplication uiApplication)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
            base.WillTerminate(uiApplication);
        }

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        #region IMvxLifetime implementation

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        #endregion IMvxLifetime implementation
    }
}

