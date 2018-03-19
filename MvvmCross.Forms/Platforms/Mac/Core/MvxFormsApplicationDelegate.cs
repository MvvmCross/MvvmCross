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
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace MvvmCross.Forms.Platform.Mac.Core
{ 
    public abstract class MvxFormsApplicationDelegate : FormsApplicationDelegate, IMvxApplicationDelegate
    {
        protected MvxFormsMacSetup Setup
        {
            get
            {
                return MvxSetup.PlatformInstance<MvxFormsMacSetup>();
            }
        }

        NSWindow window;
        public MvxFormsApplicationDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
            window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            window.TitleVisibility = NSWindowTitleVisibility.Hidden;
        }

        public override NSWindow MainWindow
        {
            get { return window; }
        }

        public override void DidFinishLaunching(Foundation.NSNotification notification)
        {
            Setup.PlatformInitialize(this, MainWindow);
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
                startup.Start(GetAppStartHint(hint));

            LoadFormsApplication();
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return null;
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
    }

    public abstract class MvxFormsApplicationDelegate<TMvxMacSetup, TApplication, TFormsApplication> : MvxFormsApplicationDelegate
    where TMvxMacSetup : MvxFormsMacSetup<TApplication, TFormsApplication>, new()
    where TApplication : IMvxApplication, new()
    where TFormsApplication : Application, new()
    {
        static MvxFormsApplicationDelegate()
        {
            MvxSetup.RegisterSetupType<TMvxMacSetup>();
        }
    }
}
