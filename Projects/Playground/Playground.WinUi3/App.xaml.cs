using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using MvvmCross.DependencyInjection;
using MvvmCross.Platforms.WinUi.Hosting;
using MvvmCross.Platforms.WinUi.Views;
using MvvmCross.Plugin.Color.Platforms.WinUi;
using MvvmCross.Plugin.Json;
using MvvmCross.Plugin.Visibility.Platforms.WinUi;
using Playground.Core;
using Playground.Core.ViewModels;

namespace Playground.WinUi
{
    public sealed partial class App : MvxApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow()
        {
            return new Window()
            {
                Title = "MvvmCross WinUI 3 Playground"
            };
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            await MvxWinUiHostBuilder.CreateBuilder(MainWindow!)
                .ConfigureServices(services =>
                {
                    services.AddMvvmCross<PlaygroundStartup>(opts =>
                        opts.StartWith<RootViewModel>()
                            .AddViewAssembly(typeof(App).Assembly));
                    services.AddMvvmCrossVisibility();
                    services.AddMvvmCrossColor();
                    services.AddMvvmCrossJson();
                })
                .Build()
                .Start();
            MainWindow!.Activate();
        }
    }
}
