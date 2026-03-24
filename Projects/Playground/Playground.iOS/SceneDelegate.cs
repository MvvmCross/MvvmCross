using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.DependencyInjection;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Platforms.Ios.Hosting;
using MvvmCross.Plugin.Color.Platforms.Ios;
using MvvmCross.Plugin.Json;
using MvvmCross.Plugin.Visibility.Platforms.Ios;
using Playground.Core;
using Playground.Core.ViewModels;

namespace Playground.iOS;

[Register("SceneDelegate")]
public class SceneDelegate : MvxSceneDelegate
{
    public override void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
    {
        Window = new UIWindow((UIWindowScene)scene);
        MvxIosHostBuilder.CreateBuilder(Window)
            .ConfigureServices(services =>
            {
                services.AddMvvmCross<PlaygroundStartup>(opts => opts.StartWith<RootViewModel>());
                services.AddMvvmCrossVisibility();
                services.AddMvvmCrossColor();
                services.AddMvvmCrossJson();
            })
            .Build()
            .Start()
            .GetAwaiter()
            .GetResult();
        Window.MakeKeyAndVisible();
    }
}
