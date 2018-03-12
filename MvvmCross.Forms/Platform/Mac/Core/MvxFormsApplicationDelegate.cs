// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using AppKit;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.Forms.Presenters;
using MvvmCross.IoC;
using MvvmCross.Platform.Mac.Core;
using MvvmCross.ViewModels;
using Xamarin.Forms.Platform.MacOS;

namespace MvvmCross.Forms.Platform.Mac.Core
{
    public abstract class MvxFormsApplicationDelegate : FormsApplicationDelegate, IMvxApplicationDelegate
    {
        private MvxFormsMacSetup _setup;
        protected MvxFormsMacSetup Setup
        {
            get
            {
                if (_setup == null)
                    _setup = CreateSetup(this, MainWindow);
                return _setup;
            }
        }

        public override void DidFinishLaunching(Foundation.NSNotification notification)
        {
            Setup.Initialize();

            RunAppStart(notification);

            Setup.FormsApplication.SendStart();
            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            base.DidFinishLaunching(notification);
        }

        protected virtual void RunAppStart(object hint = null)
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start(hint);

            LoadFormsApplication();
        }

        protected virtual void LoadFormsApplication()
        {
            LoadApplication(Setup.FormsApplication);
        }

        public override void WillBecomeActive(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.ActivatedFromMemory);
        }

        public override void DidResignActive(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Deactivated);
        }

        public override void WillTerminate(Foundation.NSNotification notification)
        {
            FireLifetimeChanged(MvxLifetimeEvent.Closing);
            base.WillTerminate(notification);
        }

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        protected virtual MvxFormsMacSetup CreateSetup(IMvxApplicationDelegate applicationDelegate, NSWindow window)
        {
            return MvxSetupExtensions.CreateSetup<MvxFormsMacSetup>(this.GetType().Assembly, applicationDelegate, window);
        }
    }
}
