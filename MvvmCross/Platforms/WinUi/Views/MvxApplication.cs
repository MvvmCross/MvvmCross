// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.WinUi.Core;
using MvvmCross.Platforms.WinUi.Views.Suspension;
using MvvmCross.ViewModels;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using MvvmCross.Platforms.WinUi.Presenters;
using Application = Microsoft.UI.Xaml.Application;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace MvvmCross.Platforms.WinUi.Views
{

    public abstract class MvxApplication : Application
    {
        private IntPtr _oldWndProc = IntPtr.Zero;
        private WinProc? _newWndProc;

        // ReSharper disable once InconsistentNaming
        private delegate IntPtr WinProc(IntPtr hWnd, PInvoke.User32.WindowMessage Msg, IntPtr wParam, IntPtr lParam);

        protected Frame RootFrame { get; set; }
        public Window MainWindow { get; protected set; }

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

        [DllImport("user32.dll")]
        // ReSharper disable once InconsistentNaming
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, PInvoke.User32.WindowMessage Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, PInvoke.User32.WindowLongIndexFlags nIndex, WinProc newProc);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, PInvoke.User32.WindowLongIndexFlags nIndex, WinProc newProc);

        // This static method is required because Win32 does not support
        // GetWindowLongPtr directly
        private static IntPtr SetWindowLongPtr(IntPtr hWnd, PInvoke.User32.WindowLongIndexFlags nIndex, WinProc newProc)
            => IntPtr.Size == 8 ? SetWindowLongPtr64(hWnd, nIndex, newProc) : SetWindowLongPtr32(hWnd, nIndex, newProc);

        private IntPtr NewWindowProc(IntPtr hWnd, PInvoke.User32.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case PInvoke.User32.WindowMessage.WM_SYSCOMMAND:
                    if (wParam.ToInt32() == (int)PInvoke.User32.SysCommands.SC_CLOSE)
                    {
                        this.ApplicationIsStoppingAsync();
                    }

                    break;
            }

            return CallWindowProc(this._oldWndProc, hWnd, msg, wParam, lParam);
        }

        protected virtual void ApplicationIsStoppingAsync()
        {
            try
            {
                if (Mvx.IoCProvider?.Resolve<IMvxWindowsViewPresenter>() is MvxMultiWindowViewPresenter presenter)
                {
                    presenter.CloseAllWindows();
                }
            }
            finally
            {
                Current.Exit();
            }
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
