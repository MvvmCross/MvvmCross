// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platforms.Tvos.Core
{
    public abstract class MvxApplicationDelegate : UIApplicationDelegate, IMvxApplicationDelegate
    {
        /// <summary>
        /// UIApplicationDelegate.Window doesn't really exist / work. It was added by Xamarin.iOS templates 
        /// </summary>
        public virtual UIWindow MainWindow { get; set; }

        protected MvxApplicationDelegate()
        {
            RegisterSetup();
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            MainWindow ??= new UIWindow(UIScreen.MainScreen.Bounds);

            MvxTvosSetupSingleton.EnsureSingletonAvailable(this, MainWindow).EnsureInitialized();
            RunAppStart(launchOptions);

            FireLifetimeChanged(MvxLifetimeEvent.Launching);
            return true;
        }

        protected virtual void RunAppStart(object hint = null)
        {
            if (Mvx.IoCProvider?.TryResolve(out IMvxAppStart startup) == true && !startup.IsStarted)
            {
                startup.Start(GetAppStartHint(hint));
            }
            MainWindow.MakeKeyAndVisible();
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return hint;
        }

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

        private void FireLifetimeChanged(MvxLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            handler?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        protected virtual void RegisterSetup()
        {
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
    }

    public abstract class MvxApplicationDelegate<TMvxTvosSetup, TApplication> : MvxApplicationDelegate
       where TMvxTvosSetup : MvxTvosSetup<TApplication>, new()
       where TApplication : class, IMvxApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxTvosSetup>();
        }
    }
}
