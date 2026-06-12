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
[RequiresUnreferencedCode("The SceneDelegate uses MvvmCross which may use reflection for view and ViewModel assembly scanning, and may call methods that require unreferenced code.")]
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
            .StartWith<RootViewModel>()
            .ConfigureServices(services =>
            {
                services.AddMvxBindings(config => config
                    .FillTargetFactories(registry =>
                        registry.RegisterCustomBindingFactory<BinaryEdit>(
                            "MyCount",
                            view => new BinaryEditTargetBinding(view))));
                services.AddLogging(l => l.AddSerilog());
                services.AddMvxMessenger();
                services.AddMvxVisibility();
                services.AddMvxColor();
                services.AddMvxJson();
                services.AddMvxViewModels(typeof(RootViewModel).Assembly);
                services.AddMvxIosViews(typeof(SceneDelegate).Assembly);
            })
            .Build()
            .Start()
            .GetAwaiter()
            .GetResult();
        Window.MakeKeyAndVisible();
    }
}
