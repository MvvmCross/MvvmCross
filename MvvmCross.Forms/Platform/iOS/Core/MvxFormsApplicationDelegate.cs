// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Foundation;
using MvvmCross.Core.Platform;
using MvvmCross.Forms.Views;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace MvvmCross.Forms.Platform.iOS.Core
{
    public class MvxFormsApplicationDelegate : FormsApplicationDelegate, IMvxApplicationDelegate
    {
        private UIWindow _window;
        public override UIWindow Window
        {
            get
            {
                return _window;
            }
            set
            {
                var fieldInfo = typeof(FormsApplicationDelegate).GetField("_window", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                fieldInfo.SetValue(this, value);
                _window = value;
            }
        }

        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Mvx.Resolve<IMvxFormsViewPresenter>().FormsApplication.SendStart();
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            return true;
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
