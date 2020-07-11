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

        protected MvxApplication()
        {
            RegisterSetup();
            EnteredBackground += OnEnteredBackground;
            LeavingBackground += OnLeavingBackground;
            Suspending += OnSuspending;
            Resuming += OnResuming;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="activationArgs">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs activationArgs)
        {
            base.OnLaunched(activationArgs);
            ActivationArguments = activationArgs;

            var rootFrame = InitializeFrame(activationArgs);

            if (activationArgs.PrelaunchActivated == false)
            {
                await RunAppStart(activationArgs).ConfigureAwait(false);
            }

            Window.Current.Activate();
        }

        protected override async void OnActivated(IActivatedEventArgs activationArgs)
        {
            base.OnActivated(activationArgs);
            ActivationArguments = activationArgs;

            var rootFrame = InitializeFrame(activationArgs);

            await RunAppStart(activationArgs).ConfigureAwait(false);

            Window.Current.Activate();
        }

        protected virtual async ValueTask RunAppStart(IActivatedEventArgs activationArgs)
        {
            var instance = MvxWindowsSetupSingleton.EnsureSingletonAvailable(RootFrame, ActivationArguments, nameof(Suspend));
            if (RootFrame.Content == null)
            {
                await instance.EnsureInitialized().ConfigureAwait(false);

                if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
                {
                    await startup.Start(GetAppStartHint(activationArgs)).ConfigureAwait(true);
                }
            }
            else
            {
                instance.PlatformSetup<MvxWindowsSetup>().UpdateActivationArguments(activationArgs);
            }
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return hint;
        }

        protected virtual Frame InitializeFrame(IActivatedEventArgs activationArgs)
        {
            if (!(Window.Current.Content is Frame rootFrame))
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
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName, e.Exception);
        }

        protected virtual async void OnEnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            try
            {
                var suspension = Mvx.IoCProvider.GetSingleton<IMvxSuspensionManager>();
                await EnteringBackground(suspension).ConfigureAwait(false);
            }
            finally
            {
                deferral.Complete();
            }
        }

        protected virtual Task EnteringBackground(IMvxSuspensionManager suspensionManager)
        {
            return suspensionManager.SaveAsync();
        }

        protected virtual async void OnLeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            try
            {
                var suspension = Mvx.IoCProvider.GetSingleton<IMvxSuspensionManager>();
                await LeaveBackground(suspension).ConfigureAwait(false);
            }
            finally
            {
                deferral.Complete();
            }
        }

        protected virtual Task LeaveBackground(IMvxSuspensionManager suspensionManager)
        {
            return Task.CompletedTask;
        }

        protected virtual Task Suspend(IMvxSuspensionManager suspensionManager)
        {
            return suspensionManager.SaveAsync();
        }

        protected virtual async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            try
            {
                var suspension = Mvx.IoCProvider.GetSingleton<IMvxSuspensionManager>();
                await Suspend(suspension).ConfigureAwait(false);
            }
            finally
            {
                deferral.Complete();
            }
        }

        protected virtual void OnResuming(object sender, object e)
        {
            var suspension = Mvx.IoCProvider.GetSingleton<IMvxSuspensionManager>();
            Resume(suspension);
        }

        protected virtual void Resume(IMvxSuspensionManager suspensionManager)
        {
            //return Task.CompletedTask;
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
