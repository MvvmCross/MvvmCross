// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Base;
using MvvmCross.DependencyInjection;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UIKit;

namespace MvvmCross.Platforms.Ios.Hosting;

/// <summary>
/// MvvmCross host builder for iOS/MacCatalyst.
/// </summary>
public class MvxIosHostBuilder : MvxHostBuilder
{
    /// <summary>
    /// Creates an <see cref="MvxIosHostBuilder"/> with the given <see cref="UIWindow"/>.
    /// The window is registered as a singleton so the view presenter can resolve it.
    /// </summary>
    public static MvxIosHostBuilder CreateBuilder(UIWindow window)
    {
        ArgumentNullException.ThrowIfNull(window);
        return new MvxIosHostBuilder(window);
    }

    private MvxIosHostBuilder(UIWindow window)
    {
        Services.TryAddSingleton(window);

        // View presenter (wraps UIWindow for navigation)
        Services.TryAddSingleton<IMvxIosViewPresenter>(new MvxIosViewPresenter(window));
        Services.TryAddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxIosViewPresenter>());

        // View dispatcher — also provides IMvxMainThreadDispatcher / IMvxMainThreadAsyncDispatcher
        // via MvxIosUIThreadDispatcher → MvxMainThreadAsyncDispatcher → MvxMainThreadDispatcher
        Services.TryAddSingleton<MvxIosViewDispatcher>();
        Services.TryAddSingleton<IMvxViewDispatcher>(
            sp => sp.GetRequiredService<MvxIosViewDispatcher>());
        Services.TryAddSingleton<IMvxMainThreadAsyncDispatcher>(
            sp => sp.GetRequiredService<MvxIosViewDispatcher>());
        Services.TryAddSingleton<IMvxMainThreadDispatcher>(
            sp => sp.GetRequiredService<MvxIosViewDispatcher>());

        // Views container — populated lazily from all IMvxIosViewRegistration entries
        Services.TryAddSingleton<MvxIosViewsContainer>(sp =>
        {
            var container = new MvxIosViewsContainer();
            var typeFinder = sp.GetService<IMvxViewModelTypeFinder>();
            foreach (var reg in sp.GetServices<IMvxIosViewRegistration>())
                reg.Apply(container, typeFinder);
            return container;
        });
        Services.TryAddSingleton<IMvxIosViewsContainer>(
            sp => sp.GetRequiredService<MvxIosViewsContainer>());
        Services.TryAddSingleton<IMvxViewsContainer>(
            sp => sp.GetRequiredService<MvxIosViewsContainer>());
        Services.TryAddSingleton<IMvxIosViewCreator>(
            sp => sp.GetRequiredService<MvxIosViewsContainer>());
        Services.TryAddSingleton<IMvxCurrentRequest>(
            sp => sp.GetRequiredService<MvxIosViewsContainer>());
    }

    /// <inheritdoc/>
    public override MvxHost Build()
    {
        // Binding registrations are deferred to Build() so that Configure* calls are included.
        var bindingBuilder = new MvxIosBindingBuilder(
            fillRegistryAction: FillTargetFactories,
            fillValueConvertersAction: FillValueConverters,
            fillValueCombinersAction: FillValueCombiners,
            fillBindingNamesAction: FillBindingNames);
#pragma warning disable IL2026
        bindingBuilder.DoRegistration(Services);
#pragma warning restore IL2026
        return base.Build();
    }

    /// <inheritdoc/>
    protected override MvxHost CreateHost(IServiceProvider serviceProvider)
        => new MvxIosHost(serviceProvider);

    /// <summary>
    /// iOS-specific host that triggers first ViewModel navigation from <see cref="Start"/>
    /// since there is no platform start screen equivalent (unlike Android's MvxStartActivity).
    /// </summary>
    private sealed class MvxIosHost : MvxHost
    {
        internal MvxIosHost(IServiceProvider services) : base(services) { }

        public override async Task Start()
        {
            // Set Current first, then run OnStartup for service configuration.
            // Unlike Android (which defers navigation to MvxStartActivity), iOS triggers
            // first navigation here where the UIWindow and UIScene are already available.
            SetAsCurrent();

            var startup = Services.GetService<IMvxStartup>();
            if (startup is not null)
                await startup.OnStartup(Services).ConfigureAwait(false);

            var appStart = Services.GetService<IMvxAppStart>();
            if (appStart is not null && !appStart.IsStarted)
                await appStart.StartAsync(null).ConfigureAwait(false);
        }
    }
}
