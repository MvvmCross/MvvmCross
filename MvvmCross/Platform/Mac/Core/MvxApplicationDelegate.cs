// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using MvvmCross.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platform.Mac.Core
{
    public class MvxApplicationDelegate : NSApplicationDelegate, IMvxApplicationDelegate
    {
        private MvxMacSetup _setup;
        protected MvxMacSetup Setup
        {
            get
            {
                if (_setup == null)
                    _setup = CreateSetup(this, MainWindow);
                return _setup;
            }
        }

        //TODO: Maybe make abstract
        public virtual NSWindow MainWindow
        {
            get {
                var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
                var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
                return new NSWindow(rect, style, NSBackingStore.Buffered, false);
            }
        }

        public override void DidFinishLaunching(Foundation.NSNotification notification)
        {
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

        protected virtual MvxMacSetup CreateSetup(IMvxApplicationDelegate applicationDelegate, NSWindow window)
        {
            return MvxSetupExtensions.CreateSetup<MvxMacSetup>(this.GetType().Assembly, applicationDelegate, window);
        }
    }
}
