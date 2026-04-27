// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.DependencyInjection;
using MvvmCross.Navigation;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Hosting;

/// <summary>
/// Abstract base for platform-specific MvvmCross host builders.
/// Platform-specific subclasses (e.g. <c>MvxAndroidHostBuilder</c>) provide
/// a static <c>CreateBuilder</c> factory method that accepts platform context.
/// </summary>
public abstract class MvxHostBuilder
{
    /// <summary>
    /// The service collection used to register framework and application services.
    /// Platform host builders pre-register platform-specific services before
    /// handing control to <see cref="ConfigureServices"/>.
    /// </summary>
    public IServiceCollection Services { get; } = new ServiceCollection();

    /// <summary>
    /// Registers the MvvmCross framework and configures <typeparamref name="TViewModel"/> as the
    /// first ViewModel to navigate to on startup.
    /// </summary>
    /// <typeparam name="TViewModel">The initial ViewModel the app should navigate to.</typeparam>
    /// <param name="configure">Optional callback for additional options such as <see cref="MvxOptions.AddViewAssembly"/>.</param>
    [RequiresUnreferencedCode("Configuring MvvmCross via StartWith<TViewModel>() stores types that are registered via reflection.")]
    public MvxHostBuilder StartWith<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
        Action<MvxOptions>? configure = null)
            where TViewModel : IMvxViewModel
    {
        Services.AddMvxCore(opts =>
        {
            opts.StartWith<TViewModel>();
            configure?.Invoke(opts);
        });
        return this;
    }

    /// <summary>
    /// Adds services to the container using the provided callback.
    /// </summary>
    public MvxHostBuilder ConfigureServices(Action<IServiceCollection> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        configure(Services);
        return this;
    }

    /// <summary>
    /// Replaces the default <see cref="IMvxNavigationService"/> registration with a custom
    /// implementation resolved via dependency injection.
    /// </summary>
    /// <typeparam name="T">The custom navigation service type.</typeparam>
    /// <remarks>
    /// Can be called before or after <see cref="StartWith{TViewModel}"/>. When called before,
    /// the default registration in <c>AddMvxCore</c> is skipped. When called after, the
    /// previously registered default is replaced.
    /// </remarks>
    public MvxHostBuilder UseNavigationService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class, IMvxNavigationService
    {
        Services.RemoveAll<IMvxNavigationService>();
        Services.AddSingleton<IMvxNavigationService, T>();
        return this;
    }

    /// <summary>
    /// Replaces the default <see cref="IMvxNavigationService"/> registration with an instance
    /// produced by the supplied factory.
    /// </summary>
    /// <typeparam name="T">The custom navigation service type.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the navigation service instance.</param>
    public MvxHostBuilder UseNavigationService<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxNavigationService
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<IMvxNavigationService>();
        Services.AddSingleton<IMvxNavigationService>(factory);
        return this;
    }

    /// <summary>
    /// Replaces the <see cref="IMvxViewPresenter"/> registration with a custom implementation
    /// resolved via dependency injection.
    /// </summary>
    /// <typeparam name="T">The custom view presenter type.</typeparam>
    /// <remarks>
    /// <para>
    /// On platforms where a dispatcher depends on a platform-specific presenter interface
    /// (e.g. <c>IMvxAndroidViewPresenter</c>), prefer the platform-builder's typed
    /// <c>UsePresenter</c> overload, which also replaces the platform-specific registration
    /// and keeps the dispatcher wired correctly.
    /// </para>
    /// <para>
    /// This method is safe for platform-agnostic scenarios (e.g. unit tests, WPF, WinUI, Mac,
    /// tvOS) where the dispatcher either does not exist or is not yet registered.
    /// </para>
    /// </remarks>
    public MvxHostBuilder UsePresenter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class, IMvxViewPresenter
    {
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxViewPresenter, T>();
        return this;
    }

    /// <summary>
    /// Replaces the <see cref="IMvxViewPresenter"/> registration with an instance produced by
    /// the supplied factory.
    /// </summary>
    /// <typeparam name="T">The custom view presenter type.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the presenter instance.</param>
    /// <remarks>
    /// See remarks on <see cref="UsePresenter{T}()"/> for platform-specific guidance.
    /// </remarks>
    public MvxHostBuilder UsePresenter<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxViewPresenter
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxViewPresenter>(factory);
        return this;
    }

    /// <summary>
    /// Replaces the <see cref="IMvxAppStart"/> registration with a custom implementation
    /// resolved via dependency injection.
    /// </summary>
    /// <typeparam name="T">The custom app start type.</typeparam>
    /// <remarks>
    /// Can be called before or after <see cref="StartWith{TViewModel}"/>. When called before,
    /// the default registration in <c>AddMvxCore</c> is skipped. When called after, the
    /// previously registered default is replaced. In either case the core MvvmCross services
    /// are only registered when <see cref="StartWith{TViewModel}"/> or
    /// <see cref="ConfigureServices"/> with <c>AddMvxCore()</c> is called.
    /// </remarks>
    public MvxHostBuilder UseAppStart<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class, IMvxAppStart
    {
        Services.RemoveAll<IMvxAppStart>();
        Services.AddSingleton<IMvxAppStart, T>();
        return this;
    }

    /// <summary>
    /// Replaces the <see cref="IMvxAppStart"/> registration with an instance produced by
    /// the supplied factory.
    /// </summary>
    /// <typeparam name="T">The custom app start type.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the app start instance.</param>
    public MvxHostBuilder UseAppStart<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxAppStart
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<IMvxAppStart>();
        Services.AddSingleton<IMvxAppStart>(factory);
        return this;
    }

    /// <summary>
    /// Replaces the <see cref="IMvxViewDispatcher"/> registration with a custom implementation
    /// resolved via dependency injection.
    /// </summary>
    /// <typeparam name="T">The custom view dispatcher type.</typeparam>
    /// <remarks>
    /// Only replaces the <see cref="IMvxViewDispatcher"/> registration. Platform-specific
    /// main-thread dispatcher aliases (<c>IMvxMainThreadAsyncDispatcher</c>,
    /// <c>IMvxMainThreadDispatcher</c>) are left untouched — those are platform threading
    /// concerns separate from view dispatching. This matches the scope of the old
    /// <c>CreateViewDispatcher()</c> override in Setup.cs.
    /// </remarks>
    public MvxHostBuilder UseViewDispatcher<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class, IMvxViewDispatcher
    {
        Services.RemoveAll<IMvxViewDispatcher>();
        Services.AddSingleton<IMvxViewDispatcher, T>();
        return this;
    }

    /// <summary>
    /// Replaces the <see cref="IMvxViewDispatcher"/> registration with an instance produced by
    /// the supplied factory.
    /// </summary>
    /// <typeparam name="T">The custom view dispatcher type.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the dispatcher instance.</param>
    /// <remarks>
    /// See remarks on <see cref="UseViewDispatcher{T}()"/> for scope details.
    /// </remarks>
    public MvxHostBuilder UseViewDispatcher<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxViewDispatcher
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<IMvxViewDispatcher>();
        Services.AddSingleton<IMvxViewDispatcher>(factory);
        return this;
    }

    /// <summary>
    /// Builds the <see cref="MvxHost"/> by finalising the service registrations
    /// and building the <see cref="IServiceProvider"/>.
    /// </summary>
    public virtual MvxHost Build()
    {
        var provider = Services.BuildServiceProvider();
        return CreateHost(provider);
    }

    /// <summary>
    /// Factory method called by <see cref="Build"/> to create the host instance.
    /// Override in a platform-specific builder subclass to return a custom <see cref="MvxHost"/>
    /// subclass, which can then override <see cref="MvxHost.Start"/> to perform post-build
    /// initialisation (the equivalent of the old <c>InitializeLastChance</c> hook).
    /// </summary>
    protected virtual MvxHost CreateHost(IServiceProvider serviceProvider)
        => new MvxHost(serviceProvider);
}

