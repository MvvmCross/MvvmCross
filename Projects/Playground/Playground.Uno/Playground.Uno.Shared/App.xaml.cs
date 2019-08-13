using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MvvmCross.Core;
using MvvmCross.Platforms.Uap.Core;
using MvvmCross.Platforms.Uap.Views;
using Uno.Extensions;

namespace Playground.Uwp
{
    public sealed partial class App
    {
        public App()
        {
#if __WASM__
            Windows.UI.Core.CoreDispatcher.HasThreadAccessOverride = true;
            MvxSetupSingleton.SupportsMultiThreadedStartup = false;
#endif

            ConfigureFilters(LogExtensionPoint.AmbientLoggerFactory);
            InitializeComponent();

            UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine("Unhandled exception:" + e.Message + "|" + e.Exception?.Message);
            Debug.WriteLine("Stack:" + e.Exception?.StackTrace);
            Debug.WriteLine("Inner exception:" + e.Exception?.InnerException?.Message);
            Debug.WriteLine("Inner stack:" + e.Exception?.InnerException?.StackTrace);
        }

        /// <summary>
        /// Configures global logging
        /// </summary>
        /// <param name="factory"></param>
        static void ConfigureFilters(ILoggerFactory factory)
        {
            factory
                .WithFilter(new FilterLoggerSettings
                    {
                        { "Uno", LogLevel.Warning },
                        { "Windows", LogLevel.Warning },

                        // Debug JS interop
                        // { "Uno.Foundation.WebAssemblyRuntime", LogLevel.Debug },

                        // Generic Xaml events
                        // { "Windows.UI.Xaml", LogLevel.Debug },
                        // { "Windows.UI.Xaml.VisualStateGroup", LogLevel.Debug },
                        // { "Windows.UI.Xaml.StateTriggerBase", LogLevel.Debug },
                        // { "Windows.UI.Xaml.UIElement", LogLevel.Debug },

                        // Layouter specific messages
                        // { "Windows.UI.Xaml.Controls", LogLevel.Debug },
                        // { "Windows.UI.Xaml.Controls.Layouter", LogLevel.Debug },
                        // { "Windows.UI.Xaml.Controls.Panel", LogLevel.Debug },
                        // { "Windows.Storage", LogLevel.Debug },

                        // Binding related messages
                        // { "Windows.UI.Xaml.Data", LogLevel.Debug },

                        // DependencyObject memory references tracking
                        // { "ReferenceHolder", LogLevel.Debug },
                    }
                )
#if DEBUG
                .AddConsole(LogLevel.Debug);
#else
				.AddConsole(LogLevel.Information);
#endif
        }
    }

    public abstract class PlaygroundApp : MvxApplication<MvxWindowsSetup<Core.App>, Core.App>
    {
    }
}
