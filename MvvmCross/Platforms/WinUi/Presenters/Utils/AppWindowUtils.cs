#nullable enable
using System.Reflection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using MvvmCross.Platforms.WinUi.Views;
using WinRT.Interop;
using Application = Microsoft.UI.Xaml.Application;

namespace MvvmCross.Platforms.WinUi.Presenters.Utils
{

    /// <summary>
    /// Extension methods for MvxWindowsPage.
    /// </summary>
    public static class AppWindowUtils
    {
        /// <summary>
        /// Try to get the <see cref="AppWindow"/> based on the currently opened window.
        /// </summary>
        /// <param name="appWindow">The AppWindow.</param>
        /// <returns>False if no AppWindow can be retrieved.</returns>
        public static bool TryGetAppWindow(out AppWindow? appWindow)
        {
            Window? mainWidow = (Application.Current as MvxApplication)?.MainWindow;
            appWindow = null;

            if (mainWidow is not null)
            {
                appWindow = GetAppWindowForCurrentWindow(mainWidow);
            }

            return appWindow is not null;
        }

        /// <summary>
        /// Sets the TitleBar of the app depending on the given parameters.
        /// </summary>
        /// <param name="appWindow">Optional appWindow.</param>
        /// <param name="iconPath">The path to the icon.</param>
        public static void SetTitleBar(AppWindow? appWindow = null, string? iconPath = null)
        {
            if (appWindow is null && !TryGetAppWindow(out appWindow))
            {
                return;
            }
            
            if (string.IsNullOrEmpty(appWindow!.Title))
            {
                appWindow.Title = Assembly.GetEntryAssembly()?.GetName().Name ?? "Bronkhorst FlowSuite 2";
            }

            if (iconPath != null)
            {
                appWindow.SetIcon(iconPath);
            }
        }

        /// <summary>
        /// Get the <see cref="AppWindow"/> based on a window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns>The appWindow.</returns>
        public static AppWindow GetAppWindowForCurrentWindow(Window window)
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(window);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }
    }
}
