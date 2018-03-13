// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Foundation;
using MvvmCross.Core;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platform.Ios.Core
{
    public abstract class MvxApplicationDelegate : UIApplicationDelegate, IMvxApplicationDelegate
    {
        protected MvxIosSetup Setup
        {
            get
            {
                return MvxSetup.PlatformInstance<MvxIosSetup>();
            }
        }

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

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            Setup.PlatformInitialize(this, Window);

            Setup.Initialize();

            RunAppStart(launchOptions);

            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            return true;
        }

        protected virtual void RunAppStart(object hint = null)
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start(GetAppStartHint(hint));

            Window.MakeKeyAndVisible();
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return null;
        }

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            handler?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
    }

    public abstract class MvxApplicationDelegate<TMvxIosSetup, TApplication> : MvxApplicationDelegate
       where TMvxIosSetup : MvxIosSetup<TApplication>, new()
       where TApplication : IMvxApplication, new()
    {
        static MvxApplicationDelegate()
        {
            MvxSetup.RegisterSetupType<TMvxIosSetup>();
        }
    }
}
