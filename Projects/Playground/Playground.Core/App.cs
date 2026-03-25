// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using MvvmCross.DependencyInjection;
using MvvmCross.Localization;
using MvvmCross.ViewModels;
using Playground.Core.Services;

namespace Playground.Core
{
    public class PlaygroundStartup : IMvxStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMvxTextProvider>(new TextProviderBuilder().TextProvider);
            // TODO: Register application services explicitly
            // (Previously used: CreatableTypes().EndingWith("Service").AsInterfaces().RegisterAsLazySingleton())
        }

        public async Task OnStartup(IServiceProvider services)
        {
            // Navigation is triggered by the platform's start activity (e.g. MvxStartActivity on Android).
            // Do not call IMvxAppStart here — it requires a live Activity context on Android.
            await Task.CompletedTask;
        }
    }
}
