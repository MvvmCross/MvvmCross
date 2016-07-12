using System;
using Xamarin.Forms.Platform.iOS;

using MvvmCross.iOS.Platform;
using MvvmCross.Core.Platform;
using UIKit;

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

