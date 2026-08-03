// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Exceptions;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Uap.Views.Suspension;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MvvmCross.Platforms.Uap.Views
{
    /// <summary>
    /// Base UAP application class for MvvmCross.
    /// Use a host builder in your <see cref="OnLaunched"/> override to initialize the framework.
    /// </summary>
    public abstract class MvxApplication : Application
    {
        protected IActivatedEventArgs? ActivationArguments { get; private set; }
        protected Frame? RootFrame { get; set; }

        protected MvxApplication()
        {
            EnteredBackground += OnEnteredBackground;
            LeavingBackground += OnLeavingBackground;
            Suspending += OnSuspending;
            Resuming += OnResuming;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            ActivationArguments = args;
            _ = InitializeFrame(args);
            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs activationArgs)
        {
            base.OnActivated(activationArgs);
            ActivationArguments = activationArgs;
            _ = InitializeFrame(activationArgs);
            Window.Current.Activate();
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

        protected virtual Frame CreateFrame() => new Frame();

        protected virtual void OnResumeFromTerminateState() { }

        protected virtual void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new MvxException($"Failed to load Page {e.SourcePageType.FullName}", e.Exception);
        }

        protected virtual async void OnEnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            try
            {
                var suspension = MvxHost.Current!.Services.GetRequiredService<IMvxSuspensionManager>();
                await EnteringBackground(suspension);
            }
            finally
            {
                deferral.Complete();
            }
        }

        protected virtual async Task EnteringBackground(IMvxSuspensionManager suspensionManager)
        {
            await suspensionManager.SaveAsync();
        }

        protected virtual async void OnLeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            try
            {
                var suspension = MvxHost.Current!.Services.GetRequiredService<IMvxSuspensionManager>();
                await LeaveBackground(suspension);
            }
            finally
            {
                deferral.Complete();
            }
        }

        protected virtual Task LeaveBackground(IMvxSuspensionManager suspensionManager) => Task.CompletedTask;

        protected virtual async Task Suspend(IMvxSuspensionManager suspensionManager)
        {
            await suspensionManager.SaveAsync();
        }

        protected virtual async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            try
            {
                var suspension = MvxHost.Current!.Services.GetRequiredService<IMvxSuspensionManager>();
                await Suspend(suspension);
            }
            finally
            {
                deferral.Complete();
            }
        }

        protected virtual void OnResuming(object sender, object e)
        {
            var suspension = MvxHost.Current!.Services.GetRequiredService<IMvxSuspensionManager>();
            Task.Run(() => Resume(suspension));
        }

        protected virtual Task Resume(IMvxSuspensionManager suspensionManager) => Task.CompletedTask;
    }
}

