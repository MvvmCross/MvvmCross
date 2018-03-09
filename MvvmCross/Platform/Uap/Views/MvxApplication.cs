// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Platform.Uap.Core;
using MvvmCross.Platform.Uap.Views.Suspension;
using MvvmCross.ViewModels;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MvvmCross.Platform.Uap.Views
{
    public abstract class MvxApplication: Application
    {
        protected MvxWindowsSetup Setup { get; set; }

        protected Frame RootFrame { get; set; }

        public MvxApplication()
        {
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

            var rootFrame = InitializeFrame(activationArgs);

            if (activationArgs.PrelaunchActivated == false) {
                StartSetup(rootFrame, activationArgs);
            }

            Window.Current.Activate();
        }
        
        protected override void OnActivated(IActivatedEventArgs activationArgs)
        {
            base.OnActivated(activationArgs);

            var rootFrame = InitializeFrame(activationArgs);

            StartSetup(rootFrame, activationArgs);

            Window.Current.Activate();
        }

        protected abstract MvxWindowsSetup CreateSetup(Frame rootFrame, IActivatedEventArgs activationArgs, string suspension);

        protected virtual void StartSetup(Frame rootFrame, IActivatedEventArgs activationArgs)
        {
            if (rootFrame.Content == null) {
                Setup = CreateSetup(rootFrame, activationArgs, nameof(Suspend));
                Setup.Initialize();

                Start(activationArgs);
            } else {
                Setup.UpdateActivationArguments(activationArgs);
            }

        }

        protected virtual void Start(IActivatedEventArgs activationArgs)
        {
            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();
        }

        protected virtual Frame InitializeFrame(IActivatedEventArgs activationArgs)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null) {
                rootFrame = CreateFrame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }

            if (activationArgs.PreviousExecutionState == ApplicationExecutionState.Terminated) {
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

            var suspension = Mvx.GetSingleton<IMvxSuspensionManager>() as MvxSuspensionManager;
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
            var suspension = Mvx.GetSingleton<IMvxSuspensionManager>() as MvxSuspensionManager;
            await Resume(suspension);
            await suspension.RestoreAsync();
        }

        protected virtual Task Resume(IMvxSuspensionManager suspensionManager)
        {
            return Task.CompletedTask;
        }
    }
}
