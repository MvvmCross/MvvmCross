// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Foundation;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platform.Ios.Core
{
    public abstract class MvxApplicationDelegate : UIApplicationDelegate, IMvxApplicationDelegate
    {
        protected MvxIosSetup _setup;

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

            CreateSetup(this, Window);
            _setup.Initialize();

            RunAppStart();

            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            return true;
        }

        protected virtual void RunAppStart()
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            if(!startup.IsStarted)
                startup.Start();

            Window.MakeKeyAndVisible();
        }        

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            handler?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        protected virtual void CreateSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            var setupType = FindSetupType();
            if (setupType == null) {
                throw new MvxException("Could not find a Setup class for application");
            }

            try {
                _setup = (MvxIosSetup)Activator.CreateInstance(setupType, applicationDelegate, window);
            } catch (Exception exception) {
                throw exception.MvxWrap("Failed to create instance of {0}", setupType.FullName);
            }
        }

        protected virtual Type FindSetupType()
        {
            var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.ExceptionSafeGetTypes()
                        where type.Name == "Setup"
                        where typeof(MvxIosSetup).IsAssignableFrom(type)
                        select type;

            return query.FirstOrDefault();
        }
    }
}
