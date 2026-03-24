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
            // Delegate first navigation to IMvxAppStart (registered via opts.StartWith<RootViewModel>())
            var appStart = services.GetService<IMvxAppStart>();
            if (appStart is { IsStarted: false })
                await appStart.Start(null).ConfigureAwait(false);
        }
    }
}
