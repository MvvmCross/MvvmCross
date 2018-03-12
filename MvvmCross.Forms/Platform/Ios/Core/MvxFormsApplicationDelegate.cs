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
        protected MvxFormsIosSetup _setup;

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

            CreateSetup(this, Window);
            _setup.Initialize();

            RunAppStart();

            Mvx.Resolve<IMvxFormsViewPresenter>().FormsApplication.SendStart();
            FireLifetimeChanged(MvxLifetimeEvent.Launching);

            //TODO: we might need to call base here
            return true;
        }

        protected virtual void RunAppStart()
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start();

            LoadFormsApplication();

            Window.MakeKeyAndVisible();
        }

        protected virtual void LoadFormsApplication()
        {
            LoadApplication(_setup.FormsApplication);
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

        protected virtual void CreateSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            var setupType = FindSetupType();
            if (setupType == null) {
                throw new MvxException("Could not find a Setup class for application");
            }

            try {
                _setup = (MvxFormsIosSetup)Activator.CreateInstance(setupType, applicationDelegate, window);
            } catch (Exception exception) {
                throw exception.MvxWrap("Failed to create instance of {0}", setupType.FullName);
            }
        }

        protected virtual Type FindSetupType()
        {
            var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.ExceptionSafeGetTypes()
                        where type.Name == "Setup"
                        where typeof(MvxFormsIosSetup).IsAssignableFrom(type)
                        select type;

            return query.FirstOrDefault();
        }
    }
}
