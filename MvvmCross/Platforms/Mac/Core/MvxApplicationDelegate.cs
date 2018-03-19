﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using MvvmCross.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Mac.Core
{
    public abstract class MvxApplicationDelegate : NSApplicationDelegate, IMvxApplicationDelegate
    {
        protected IMvxMacSetup Setup
        {
            get
            {
                return MvxSetup.PlatformInstance<IMvxMacSetup>();
            }
        }

        private NSWindow window;
        public virtual NSWindow MainWindow
        {
            get {
                if (window == null)
                {
                    var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
                    var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
                    window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
                }
                return window;
            }
            set { window = value; }
        }

        public override void DidFinishLaunching(Foundation.NSNotification notification)
        {
            Setup.PlatformInitialize(this, MainWindow);
            Setup.Initialize();
            RunAppStart(notification);

            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            base.DidFinishLaunching(notification);
        }

        protected virtual void RunAppStart(object hint = null)
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start(GetAppStartHint(hint));
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return null;
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

    public class MvxApplicationDelegate<TMvxMacSetup, TApplication> : MvxApplicationDelegate
   where TMvxMacSetup : MvxMacSetup<TApplication>, new()
   where TApplication : IMvxApplication, new()
    {
        static MvxApplicationDelegate()
        {
            MvxSetup.RegisterSetupType<TMvxMacSetup>();
        }
    }
}
