#nullable enable

using System.Diagnostics.CodeAnalysis;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.DependencyInjection;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Platforms.Ios.Hosting;
using MvvmCross.Platforms.Ios.Views;
using Playground.Core.ViewModels.MultiWindow;
using Playground.iOS.Views.MultiWindow;
using UIKit;

namespace Playground.iOS.MultiWindow;

[Register("MultiWindowAppDelegate")]
[RequiresUnreferencedCode("The demo uses reflection for view and ViewModel assembly scanning and navigation.")]
public class MultiWindowAppDelegate : MvxSceneApplicationDelegate
{
    public MvxHost Host { get; private set; } = null!;

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // One host and one set of shared services for the process. Each scene starts its own root.
        Host = MvxIosHostBuilder.CreateBuilder()
            .ConfigureServices(services =>
            {
                services.AddMvxCore();
                services.AddMvxBindings();
                services.AddMvxViewModels(typeof(WindowViewModel).Assembly);
                services.AddMvxIosViews(typeof(WindowView).Assembly);
                services.AddSingleton<SharedCounter>();
            })
            .Build();
        Host.Start().GetAwaiter().GetResult();
        return true;
    }
}
