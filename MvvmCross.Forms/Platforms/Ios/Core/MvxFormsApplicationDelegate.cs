// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using Foundation;
using MvvmCross.Core;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.ViewModels;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace MvvmCross.Forms.Platforms.Ios.Core
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

        public MvxFormsApplicationDelegate() : base()
        {
            RegisterSetup();
        }

        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            if (Window == null)
                Window = new UIWindow(UIScreen.MainScreen.Bounds);

            var instance = MvxIosSetupSingleton.EnsureSingletonAvailable(this, Window);
            instance.EnsureInitialized();

            RunAppStart(launchOptions);

            instance.PlatformSetup<MvxFormsIosSetup>().FormsApplication.SendStart();
            FireLifetimeChanged(MvxLifetimeEvent.Launching);

            //TODO: we don't call base for now, but we might need to as soon as Forms opens up
            return true;
        }

        protected virtual void RunAppStart(object hint = null)
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start(GetAppStartHint(hint));

            LoadFormsApplication();
            Window.MakeKeyAndVisible();
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return null;
        }

        protected virtual void LoadFormsApplication()
        {
            var instance = MvxIosSetupSingleton.EnsureSingletonAvailable(this, Window);
            LoadApplication(instance.PlatformSetup<MvxFormsIosSetup>().FormsApplication);
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

        protected virtual void RegisterSetup()
        {
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
    }

    public abstract class MvxFormsApplicationDelegate<TMvxIosSetup, TApplication, TFormsApplication> : MvxFormsApplicationDelegate
        where TMvxIosSetup : MvxFormsIosSetup<TApplication, TFormsApplication>, new()
        where TApplication : class, IMvxApplication, new()
        where TFormsApplication : Application, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxIosSetup>();
        }
    }
}
