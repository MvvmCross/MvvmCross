// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Android.Runtime;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.DependencyInjection;
using MvvmCross.Platforms.Android.Hosting;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Color.Platforms.Android;
using MvvmCross.Plugin.Json;
using MvvmCross.Plugin.Visibility.Platforms.Android;
using Playground.Core;
using Playground.Droid.Bindings;
using Playground.Droid.Controls;
using Serilog;
using Serilog.Extensions.Logging;

namespace Playground.Droid
{
    [Application]
    public class MainApplication : MvxAndroidApplication
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Async(a => a.AndroidLog())
                .WriteTo.Async(a => a.Trace())
                .CreateLogger();

            MvxAndroidHostBuilder.CreateBuilder(this)
                .ConfigureTargetBindings(registry =>
                    registry.RegisterCustomBindingFactory<BinaryEdit>(
                        "MyCount",
                        view => new BinaryEditTargetBinding(view)))
                .ConfigureServices(services =>
                {
                    services.AddLogging(l => l.AddSerilog());
                    services.AddMvvmCross<PlaygroundStartup>(opts =>
                        opts.StartWith<Playground.Core.ViewModels.RootViewModel>());
                    services.AddMvxVisibility();
                    services.AddMvxColor();
                    services.AddMvxJson();
                    // Register ViewModels from the Core assembly for name-based lookup.
                    services.AddMvxViewModels(typeof(PlaygroundStartup).Assembly);
                    // Register all MvvmCross views (Activities and Fragments) in this assembly.
                    services.AddMvxAndroidViews(typeof(MainApplication).Assembly);
                })
                .Build()
                .Start();
        }
    }
}
