// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Base;
using MvvmCross.Hosting;
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

    /// <summary>
    /// Replaces the default <see cref="IMvxIosViewPresenter"/> registration with a custom
    /// implementation resolved via dependency injection.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxIosViewPresenter"/>.</typeparam>
    /// <remarks>
    /// Also replaces the <see cref="IMvxViewPresenter"/> alias so navigation service and
    /// dispatcher both resolve the same instance. Call this before
    /// <see cref="MvxHostBuilder.StartWith{TViewModel}"/> or
    /// <see cref="MvxHostBuilder.ConfigureServices"/> to keep the platform-typed fluent chain.
    /// </remarks>
    public MvxIosHostBuilder UsePresenter<T>()
        where T : class, IMvxIosViewPresenter
    {
        Services.RemoveAll<IMvxIosViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxIosViewPresenter, T>();
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxIosViewPresenter>());
        return this;
    }

    /// <summary>
    /// Replaces the default <see cref="IMvxIosViewPresenter"/> registration with an instance
    /// produced by the supplied factory.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxIosViewPresenter"/>.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the presenter instance.</param>
    public MvxIosHostBuilder UsePresenter<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxIosViewPresenter
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<IMvxIosViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxIosViewPresenter>(factory);
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxIosViewPresenter>());
        return this;
    }

    /// <summary>
    /// Replaces the default <see cref="MvxIosViewsContainer"/> registration with a custom
    /// implementation resolved via dependency injection.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxIosViewsContainer"/>.</typeparam>
    /// <remarks>
    /// <see cref="IMvxIosViewsContainer"/> inherits <see cref="IMvxViewsContainer"/> and
    /// <see cref="IMvxIosViewCreator"/> (which in turn inherits <see cref="IMvxCurrentRequest"/>),
    /// so a single type constraint is sufficient. All alias registrations are replaced so that
    /// <see cref="IMvxIosViewsContainer"/>, <see cref="IMvxViewsContainer"/>,
    /// <see cref="IMvxIosViewCreator"/>, and <see cref="IMvxCurrentRequest"/> all resolve to
    /// the same custom instance.
    /// </remarks>
    public MvxIosHostBuilder UseViewsContainer<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class, IMvxIosViewsContainer
    {
        Services.RemoveAll<MvxIosViewsContainer>();
        Services.RemoveAll<IMvxIosViewsContainer>();
        Services.RemoveAll<IMvxViewsContainer>();
        Services.RemoveAll<IMvxIosViewCreator>();
        Services.RemoveAll<IMvxCurrentRequest>();
        Services.AddSingleton<T>();
        Services.AddSingleton<IMvxIosViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxIosViewCreator>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxCurrentRequest>(sp => sp.GetRequiredService<T>());
        return this;
    }

    /// <summary>
    /// Replaces the default <see cref="MvxIosViewsContainer"/> registration with an instance
    /// produced by the supplied factory.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxIosViewsContainer"/>.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the container instance.</param>
    public MvxIosHostBuilder UseViewsContainer<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxIosViewsContainer
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<MvxIosViewsContainer>();
        Services.RemoveAll<IMvxIosViewsContainer>();
        Services.RemoveAll<IMvxViewsContainer>();
        Services.RemoveAll<IMvxIosViewCreator>();
        Services.RemoveAll<IMvxCurrentRequest>();
        Services.AddSingleton(factory);
        Services.AddSingleton<IMvxIosViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxIosViewCreator>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxCurrentRequest>(sp => sp.GetRequiredService<T>());
        return this;
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
            // Unlike Android (which defers navigation to MvxStartActivity), iOS triggers
            // first navigation here where the UIWindow and UIScene are already available.
            SetAsCurrent();

            var appStart = Services.GetService<IMvxAppStart>();
            if (appStart is not null && !appStart.IsStarted)
                await appStart.StartAsync(null).ConfigureAwait(false);
        }
    }
}
