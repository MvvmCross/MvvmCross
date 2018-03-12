// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.Forms.Presenters;
using MvvmCross.IoC;
using MvvmCross.Platform.Ios.Core;
using MvvmCross.ViewModels;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace MvvmCross.Forms.Platform.Ios.Core
{
    public abstract class MvxFormsApplicationDelegate : FormsApplicationDelegate, IMvxApplicationDelegate
    {
        private MvxFormsIosSetup _setup;
        protected MvxFormsIosSetup Setup { get
            {
                if (_setup == null)
                    _setup = CreateSetup(this, Window);
                return _setup;
            }
        }

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
            Setup.Initialize();

            RunAppStart(launchOptions);

            Setup.FormsApplication.SendStart();
            FireLifetimeChanged(MvxLifetimeEvent.Launching);

            //TODO: we might need to call base here
            return true;
        }

        protected virtual void RunAppStart(object hint = null)
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start(hint);

            LoadFormsApplication();
            Window.MakeKeyAndVisible();
        }

        protected virtual void LoadFormsApplication()
        {
            LoadApplication(Setup.FormsApplication);
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

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        protected virtual MvxFormsIosSetup CreateSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            return MvxSetupExtensions.CreateSetup<MvxFormsIosSetup>(this.GetType().Assembly, applicationDelegate, window);
        }
    }
}
