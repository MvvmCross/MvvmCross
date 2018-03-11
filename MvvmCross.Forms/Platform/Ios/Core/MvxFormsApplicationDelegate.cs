// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Foundation;
using MvvmCross.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platform.Ios.Core;
using MvvmCross.ViewModels;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace MvvmCross.Forms.Platform.Ios.Core
{
    public abstract class MvxFormsApplicationDelegate : FormsApplicationDelegate, IMvxApplicationDelegate
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
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(65, 105, 225);
            UINavigationBar.Appearance.TintColor = UIColor.FromRGB(255, 255, 255);

            var setup = CreateSetup(this, Window);
            setup.Initialize();

            // TODO: Do we need to force this to run synchronously?
            CompleteFormsSetup();

            Mvx.Resolve<IMvxFormsViewPresenter>().FormsApplication.SendStart();
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            return true;
        }


        protected virtual async Task CompleteFormsSetup()
        {
            await StartSetup();

            LoadFormsApplication();

            Window.MakeKeyAndVisible();
        }

        protected virtual async Task StartSetup()
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            if (startup is IMvxAppStartAsync waitAppStart) {
                await waitAppStart.WaitForStart();
            }
        }

        protected virtual void LoadFormsApplication()
        {
            var presenter = Mvx.Resolve<IMvxFormsViewPresenter>();
            LoadApplication(presenter.FormsApplication);
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

        protected abstract MvxIosSetup CreateSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window);


        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        #region IMvxLifetime implementation

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        #endregion IMvxLifetime implementation
    }
}
