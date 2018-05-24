// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Core;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Views.Suspension;
using MvvmCross.ViewModels;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MvvmCross.Platforms.Uap.Views
{
    public abstract class MvxApplication : Application
    {
        protected IActivatedEventArgs ActivationArguments { get; private set; }

        protected Frame RootFrame { get; set; }

        public MvxApplication()
        {
            RegisterSetup();
            Suspending += OnSuspending;
            Resuming += OnResuming;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="activationArgs">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs activationArgs)
        {
            base.OnLaunched(activationArgs);
            ActivationArguments = activationArgs;

            var rootFrame = InitializeFrame(activationArgs);

            if (activationArgs.PrelaunchActivated == false)
            {
                RunAppStart(activationArgs);
            }

            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs activationArgs)
        {
            base.OnActivated(activationArgs);
            ActivationArguments = activationArgs;

            var rootFrame = InitializeFrame(activationArgs);
            RunAppStart(activationArgs);

            Window.Current.Activate();
        }

        protected virtual void RunAppStart(IActivatedEventArgs activationArgs)
        {
            var instance = MvxWindowsSetupSingleton.EnsureSingletonAvailable(RootFrame, ActivationArguments, nameof(Suspend));
            if (RootFrame.Content == null)
            {
                instance.EnsureInitialized();

                var startup = Mvx.Resolve<IMvxAppStart>();
                if (!startup.IsStarted)
                    startup.Start(GetAppStartHint(activationArgs));
            }
            else
            {
                instance.PlatformSetup<MvxWindowsSetup>().UpdateActivationArguments(activationArgs);
            }
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return null;
        }

        protected virtual Frame InitializeFrame(IActivatedEventArgs activationArgs)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = CreateFrame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }

            if (activationArgs.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                OnResumeFromTerminateState();
            }

            RootFrame = rootFrame;

            return rootFrame;
        }

        protected virtual Frame CreateFrame()
        {
            return new Frame();
        }

        protected virtual void OnResumeFromTerminateState()
        {
        }

        protected virtual void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            var suspension = Mvx.GetSingleton<IMvxSuspensionManager>();
            await Suspend(suspension);
            await suspension.SaveAsync();
            deferral.Complete();
        }

        protected virtual Task Suspend(IMvxSuspensionManager suspensionManager)
        {
            return Task.CompletedTask;
        }

        private async void OnResuming(object sender, object e)
        {
            var suspension = Mvx.GetSingleton<IMvxSuspensionManager>();
            await Resume(suspension);
            await suspension.RestoreAsync();
        }

        protected virtual Task Resume(IMvxSuspensionManager suspensionManager)
        {
            return Task.CompletedTask;
        }

        protected virtual void RegisterSetup()
        {
        }
    }

    public class MvxApplication<TMvxUapSetup, TApplication> : MvxApplication
       where TMvxUapSetup : MvxWindowsSetup<TApplication>, new()
       where TApplication : class, IMvxApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxUapSetup>();
        }
    }
}
