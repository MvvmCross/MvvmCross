using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.DependencyInjection;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Platforms.Ios.Hosting;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Plugin.Color.Platforms.Ios;
using MvvmCross.Plugin.Json;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Plugin.Visibility.Platforms.Ios;
using Playground.Core;
using Playground.Core.ViewModels;
using Playground.iOS.Bindings;
using Playground.iOS.Controls;
using Serilog;

namespace Playground.iOS;

[Register("SceneDelegate")]
public class SceneDelegate : MvxSceneDelegate
{
    public override void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Async(a => a.NSLog())
            .WriteTo.Async(a => a.Trace())
            .CreateLogger();

        Window = new UIWindow((UIWindowScene)scene);
        MvxIosHostBuilder.CreateBuilder(Window)
            .ConfigureTargetBindings(registry =>
                registry.RegisterCustomBindingFactory<BinaryEdit>(
                    "MyCount",
                    view => new BinaryEditTargetBinding(view)))
            .ConfigureServices(services =>
            {
                services.AddLogging(l => l.AddSerilog());
                services.AddMvvmCross<PlaygroundStartup>(opts => opts.StartWith<RootViewModel>());
                services.AddMvvmCrossMessenger();
                services.AddMvvmCrossVisibility();
                services.AddMvvmCrossColor();
                services.AddMvvmCrossJson();
                services.AddMvvmCrossViewModels(typeof(PlaygroundStartup).Assembly);
                services.AddMvvmCrossIosViews(typeof(SceneDelegate).Assembly);
            })
            .Build()
            .Start()
            .GetAwaiter()
            .GetResult();
        Window.MakeKeyAndVisible();
    }
}
