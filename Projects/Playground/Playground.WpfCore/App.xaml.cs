using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.DependencyInjection;
using MvvmCross.Platforms.Wpf.Hosting;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Plugin.Color.Platforms.Wpf;
using MvvmCross.Plugin.Json;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Plugin.Visibility.Platforms.Wpf;
using Playground.Core;
using Playground.Core.ViewModels;

namespace Playground.WpfCore
{
    public partial class App : MvxApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = new MainWindow();
            MvxWpfHostBuilder.CreateBuilder(mainWindow)
                .ConfigureServices(services =>
                {
                    services.AddMvvmCross<PlaygroundStartup>(opts =>
                        opts.StartWith<RootViewModel>()
                            .AddViewAssembly(typeof(App).Assembly));
                    services.AddMvxVisibility();
                    services.AddMvxColor();
                    services.AddMvxMessenger();
                    services.AddMvxJson();
                })
                .Build()
                .Start()
                .GetAwaiter()
                .GetResult();
            mainWindow.Show();
        }
    }
}
