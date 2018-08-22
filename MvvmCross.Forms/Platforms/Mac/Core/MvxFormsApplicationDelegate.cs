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
using MvvmCross.Platforms.Mac.Core;
using MvvmCross.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace MvvmCross.Forms.Platforms.Mac.Core
{
    public abstract class MvxFormsApplicationDelegate : FormsApplicationDelegate, IMvxApplicationDelegate
    {
        private NSWindow window;
        public override NSWindow MainWindow
        {
            get
            {
                if (window == null)
                {
                    var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

                    var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
                    window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
                    window.TitleVisibility = NSWindowTitleVisibility.Hidden;
                }
                return window;
            }
        }

        public MvxFormsApplicationDelegate() : base()
        {
            RegisterSetup();
        }

        public override void DidFinishLaunching(Foundation.NSNotification notification)
        {
            var instance = MvxMacSetupSingleton.EnsureSingletonAvailable(this, MainWindow);
            instance.EnsureInitialized();

            RunAppStart(notification);

            //TODO: This is also called in the base, maybe we need to remove it
            instance.PlatformSetup<MvxFormsMacSetup>().FormsApplication.SendStart();
            FireLifetimeChanged(MvxLifetimeEvent.Launching);

            // Unlike most other overrides, this should be left here so that the base FormsApplicationDelegate override is called
            base.DidFinishLaunching(notification);
        }

        protected virtual void RunAppStart(object hint = null)
        {
            var startup = Mvx.IoCProvider.Resolve<IMvxAppStart>();
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
            var instance = MvxMacSetupSingleton.EnsureSingletonAvailable(this, MainWindow);
            LoadApplication(instance.PlatformSetup<MvxFormsMacSetup>().FormsApplication);
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

    public abstract class MvxFormsApplicationDelegate<TMvxMacSetup, TApplication, TFormsApplication> : MvxFormsApplicationDelegate
    where TMvxMacSetup : MvxFormsMacSetup<TApplication, TFormsApplication>, new()
    where TApplication : class, IMvxApplication, new()
    where TFormsApplication : Application, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxMacSetup>();
        }
    }
}
