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
    public abstract class MvxBaseApplication: Application
    {
        protected MvxWindowsSetup InternalSetup { get; set; }

        public MvxBaseApplication()
        {
            Suspending += OnSuspending;
            Resuming += OnResuming;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);

            var rootFrame = InitializeFrame(e);

            if (e.PrelaunchActivated == false) {
                Setup(rootFrame);
            }

            Window.Current.Activate();
        }
        
        protected override void OnActivated(IActivatedEventArgs e)
        {
            base.OnActivated(e);

            var rootFrame = InitializeFrame(e);

            Setup(rootFrame);

            Window.Current.Activate();
        }

        protected abstract MvxWindowsSetup CreateSetup(Frame rootFrame, string suspension);

        protected virtual void Setup(Frame rootFrame)
        {
            if (rootFrame.Content == null) {
                InternalSetup = CreateSetup(rootFrame, nameof(Suspend));
                InternalSetup.Initialize();

                Start();
            }

        }

        protected virtual void Start()
        {
            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();
        }

        protected virtual Frame InitializeFrame(IActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null) {
                rootFrame = CreateFrame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                OnResumeFromTerminateState();
            }

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
