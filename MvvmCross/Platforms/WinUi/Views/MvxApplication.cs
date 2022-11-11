// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.WinUi.Core;
using MvvmCross.Platforms.WinUi.Views.Suspension;
using MvvmCross.ViewModels;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace MvvmCross.Platforms.WinUi.Views
{
    public abstract class MvxApplication : Application
    {
        protected Frame RootFrame { get; set; }
        protected Window MainWindow { get; set; }

        protected MvxApplication()
        {
            RegisterSetup();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _ = InitializeFrame(args.Arguments);

            RunAppStart(args.Arguments);

            MainWindow.Activate();
        }

        protected virtual void RunAppStart(string arguments)
        {
            var instance = MvxWindowsSetupSingleton.EnsureSingletonAvailable(RootFrame, arguments, "Suspend");

            if (RootFrame.Content == null)
            {
                instance.EnsureInitialized();

                if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
                {
                    startup.Start(GetAppStartHint(arguments));
                }
            }
        }

        protected virtual object GetAppStartHint(object hint = null)
        {
            return hint;
        }

        protected virtual Window CreateWindow()
        {
            return new Window();
        }

        protected virtual Frame InitializeFrame(string arguments)
        {
            if (MainWindow == null)
            {
                MainWindow = CreateWindow();
            }

            var rootFrame = MainWindow.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = CreateFrame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                MainWindow.Content = rootFrame;
            }

            RootFrame = rootFrame;

            return rootFrame;
        }

        protected virtual Frame CreateFrame()
        {
            return new Frame();
        }

        protected virtual void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new MvxException($"Failed to load Page {e.SourcePageType.FullName}", e.Exception);
        }

        protected virtual void RegisterSetup()
        {

        }
    }

    public class MvxApplication<TMvxWinUiSetup, TApplication> : MvxApplication
       where TMvxWinUiSetup : MvxWindowsSetup<TApplication>, new()
       where TApplication : class, IMvxApplication, new()
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxWinUiSetup>();
        }
    }
}
