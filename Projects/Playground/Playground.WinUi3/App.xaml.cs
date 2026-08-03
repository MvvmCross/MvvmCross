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
                .StartWith<RootViewModel>(opts => opts.AddViewAssembly(typeof(App).Assembly))
                .ConfigureServices(services =>
                {
                    services.AddMvxBindings();
                    services.AddMvxVisibility();
                    services.AddMvxColor();
                    services.AddMvxJson();
                })
                .Build()
                .Start();
            MainWindow!.Activate();
        }
    }
}
