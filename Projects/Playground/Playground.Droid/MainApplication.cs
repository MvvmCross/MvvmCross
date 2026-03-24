// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Runtime;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.DependencyInjection;
using MvvmCross.Platforms.Android.Hosting;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Color.Platforms.Android;
using MvvmCross.Plugin.Json;
using MvvmCross.Plugin.Visibility.Platforms.Android;
using Playground.Core;
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
                .ConfigureServices(services =>
                {
                    services.AddLogging(l => l.AddSerilog());
                    services.AddMvvmCross<PlaygroundStartup>(opts =>
                        opts.StartWith<Playground.Core.ViewModels.RootViewModel>());
                    services.AddMvvmCrossVisibility();
                    services.AddMvvmCrossColor();
                    services.AddMvvmCrossJson();
                })
                .Build()
                .Start();
        }
    }
}
