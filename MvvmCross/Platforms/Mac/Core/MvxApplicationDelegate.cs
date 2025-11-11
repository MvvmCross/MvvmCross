// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using AppKit;
using MvvmCross.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Mac.Core
{
    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public abstract class MvxApplicationDelegate : NSApplicationDelegate, IMvxApplicationDelegate
    {
        protected MvxApplicationDelegate() : base()
        {
            RegisterSetup();
        }

        public override void DidFinishLaunching(Foundation.NSNotification notification)
        {
            MvxMacSetupSingleton.EnsureSingletonAvailable(this).EnsureInitialized();
            RunAppStart(notification);

            FireLifetimeChanged(MvxLifetimeEvent.Launching);
        }

        protected virtual void RunAppStart(object hint = null)
        {
            if (Mvx.IoCProvider?.TryResolve(out IMvxAppStart startup) == true && !startup.IsStarted)
            {
                startup.Start(GetAppStartHint(hint));
            }
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return hint;
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

    [RequiresUnreferencedCode("This class may use types that are not preserved by trimming")]
    public class MvxApplicationDelegate<TMvxMacSetup, TApplication> : MvxApplicationDelegate
        where TMvxMacSetup : MvxMacSetup<TApplication>, new()
        where TApplication : class, IMvxApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxMacSetup>();
        }
    }
}
