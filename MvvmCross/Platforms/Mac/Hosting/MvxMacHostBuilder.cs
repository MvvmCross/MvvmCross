// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Base;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Mac.Presenters;
using MvvmCross.Platforms.Mac.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Mac.Hosting;

/// <summary>
/// MvvmCross host builder for macOS.
/// </summary>
public class MvxMacHostBuilder : MvxHostBuilder
{
    /// <summary>
    /// Creates an <see cref="MvxMacHostBuilder"/>.
    /// </summary>
    public static MvxMacHostBuilder CreateBuilder()
    {
        return new MvxMacHostBuilder();
    }

    private MvxMacHostBuilder()
    {
        Services.TryAddSingleton(NSApplication.SharedApplication.Delegate);
        UsePresenter<MvxMacViewPresenter>();
        UseViewDispatcher<MvxMacViewDispatcher>();

        Services.TryAddSingleton<IMvxMainThreadAsyncDispatcher>(
            sp => sp.GetRequiredService<IMvxViewDispatcher>());
        Services.TryAddSingleton<IMvxMainThreadDispatcher>(
            sp => sp.GetRequiredService<IMvxViewDispatcher>());

        // Views container — populated lazily from all IMvxMacViewRegistration entries
        UseViewsContainer<MvxMacViewsContainer>(sp =>
        {
            var container = new MvxMacViewsContainer();
            var typeFinder = sp.GetService<IMvxViewModelTypeFinder>();
            foreach (var reg in sp.GetServices<IMvxMacViewRegistration>())
                reg.Apply(container, typeFinder);
            return container;
        });
    }

    /// <summary>
    /// Registers a custom <see cref="IMvxMacViewPresenter"/> implementation resolved via
    /// dependency injection, replacing any previously registered presenter.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxMacViewPresenter"/>.</typeparam>
    /// <remarks>
    /// Also registers <see cref="IMvxViewPresenter"/> as an alias pointing to the same instance.
    /// Call this before <see cref="MvxHostBuilder.StartWith{TViewModel}"/> or
    /// <see cref="MvxHostBuilder.ConfigureServices"/> to keep the platform-typed fluent chain.
    /// </remarks>
    public MvxMacHostBuilder UsePresenter<T>()
        where T : class, IMvxMacViewPresenter
    {
        Services.RemoveAll<IMvxMacViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxMacViewPresenter, T>();
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxMacViewPresenter>());
        return this;
    }

    /// <summary>
    /// Registers a custom <see cref="IMvxMacViewPresenter"/> produced by the supplied factory,
    /// replacing any previously registered presenter.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxMacViewPresenter"/>.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the presenter instance.</param>
    public MvxMacHostBuilder UsePresenter<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxMacViewPresenter
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<IMvxMacViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxMacViewPresenter>(factory);
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxMacViewPresenter>());
        return this;
    }

    /// <summary>
    /// Replaces the default <see cref="MvxMacViewsContainer"/> registration with a custom
    /// implementation resolved via dependency injection.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxMacViewsContainer"/>.</typeparam>
    /// <remarks>
    /// <see cref="IMvxMacViewsContainer"/> inherits <see cref="IMvxViewsContainer"/> and
    /// <see cref="IMvxMacViewCreator"/> (which in turn inherits <see cref="IMvxCurrentRequest"/>),
    /// so a single type constraint is sufficient. All alias registrations are replaced so that
    /// <see cref="IMvxMacViewsContainer"/>, <see cref="IMvxViewsContainer"/>,
    /// <see cref="IMvxMacViewCreator"/>, and <see cref="IMvxCurrentRequest"/> all resolve to
    /// the same custom instance.
    /// </remarks>
    public MvxMacHostBuilder UseViewsContainer<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class, IMvxMacViewsContainer
    {
        Services.RemoveAll<MvxMacViewsContainer>();
        Services.RemoveAll<IMvxMacViewsContainer>();
        Services.RemoveAll<IMvxViewsContainer>();
        Services.RemoveAll<IMvxMacViewCreator>();
        Services.RemoveAll<IMvxCurrentRequest>();
        Services.AddSingleton<T>();
        Services.AddSingleton<IMvxMacViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxMacViewCreator>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxCurrentRequest>(sp => sp.GetRequiredService<T>());
        return this;
    }

    /// <summary>
    /// Replaces the default <see cref="MvxMacViewsContainer"/> registration with an instance
    /// produced by the supplied factory.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxMacViewsContainer"/>.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the container instance.</param>
    public MvxMacHostBuilder UseViewsContainer<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxMacViewsContainer
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<MvxMacViewsContainer>();
        Services.RemoveAll<IMvxMacViewsContainer>();
        Services.RemoveAll<IMvxViewsContainer>();
        Services.RemoveAll<IMvxMacViewCreator>();
        Services.RemoveAll<IMvxCurrentRequest>();
        Services.AddSingleton(factory);
        Services.AddSingleton<IMvxMacViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxMacViewCreator>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxCurrentRequest>(sp => sp.GetRequiredService<T>());
        return this;
    }
}
