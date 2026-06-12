// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using MvvmCross.Exceptions;
using Application = Microsoft.UI.Xaml.Application;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace MvvmCross.Platforms.WinUi.Views
{
    /// <summary>
    /// Base WinUI application class for MvvmCross.
    /// Use <see cref="MvvmCross.Platforms.WinUi.Hosting.MvxWinUiHostBuilder"/> in your
    /// <see cref="OnLaunched"/> override to initialize the framework.
    /// </summary>
    public abstract class MvxApplication : Application
    {
        protected Frame? RootFrame { get; set; }
        public Window? MainWindow { get; protected set; }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            InitializeFrame(args.Arguments);
        }

        protected virtual Window CreateWindow() => new Window();

        protected virtual Frame InitializeFrame(string arguments)
        {
            MainWindow ??= CreateWindow();

            if (MainWindow.Content is not Frame rootFrame)
            {
                rootFrame = CreateFrame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                MainWindow.Content = rootFrame;
            }

            RootFrame = rootFrame;
            return rootFrame;
        }

        protected virtual Frame CreateFrame() => new Frame();

        protected virtual void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new MvxException($"Failed to load Page {e.SourcePageType.FullName}", e.Exception);
        }
    }
}

