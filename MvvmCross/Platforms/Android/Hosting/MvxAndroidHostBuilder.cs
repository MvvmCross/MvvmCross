// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Android.App;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Base;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Hosting;

/// <summary>
/// MvvmCross host builder for Android. Creates an <see cref="MvxHost"/> pre-wired with
/// Android platform context and services.
/// </summary>
public class MvxAndroidHostBuilder : MvxHostBuilder
{

    /// <summary>
    /// Creates an <see cref="MvxAndroidHostBuilder"/> for the given Android <see cref="Application"/>.
    /// The application instance is registered as a singleton so it can be resolved by services that
    /// need the Android context.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Building an Android MvvmCross host registers services that may use reflection for view and ViewModel assembly scanning.")]
    public static MvxAndroidHostBuilder CreateBuilder(Application application)
    {
        ArgumentNullException.ThrowIfNull(application);
        return new MvxAndroidHostBuilder(application);
    }

    [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Registering Android view container factory uses reflection via IMvxAndroidViewRegistration.Apply which may scan assemblies.")]
    private MvxAndroidHostBuilder(Application application)
    {
        // Android application context
        Services.TryAddSingleton(application);
        Services.TryAddSingleton<global::Android.Content.Context>(application);

        // Track the current top activity via Android Application lifecycle callbacks.
        // Must be registered immediately (not lazily) so it receives events from the very first Activity.
        var currentTopActivity = new MvxCurrentTopActivity();
        application.RegisterActivityLifecycleCallbacks(currentTopActivity);
        Services.TryAddSingleton<IMvxAndroidCurrentTopActivity>(currentTopActivity);

        // Activity lifetime tracking (used by presenter and other components)
        Services.TryAddSingleton<MvxAndroidLifetimeMonitor>();
        Services.TryAddSingleton<IMvxAndroidActivityLifetimeListener>(
            sp => sp.GetRequiredService<MvxAndroidLifetimeMonitor>());

        // Main thread dispatcher
        Services.TryAddSingleton<MvxAndroidMainThreadDispatcher>();
        Services.TryAddSingleton<IMvxMainThreadAsyncDispatcher>(
            sp => sp.GetRequiredService<MvxAndroidMainThreadDispatcher>());
        Services.TryAddSingleton<IMvxMainThreadDispatcher>(
            sp => sp.GetRequiredService<MvxAndroidMainThreadDispatcher>());

        // Intent result sink (for Activity.StartActivityForResult flows)
        Services.TryAddSingleton<MvxIntentResultSink>();
        Services.TryAddSingleton<IMvxIntentResultSink>(sp => sp.GetRequiredService<MvxIntentResultSink>());
        Services.TryAddSingleton<IMvxIntentResultSource>(sp => sp.GetRequiredService<MvxIntentResultSink>());

        // Single-ViewModel cache (for re-using ViewModel instances across config changes)
        Services.TryAddSingleton<IMvxSingleViewModelCache, MvxSingleViewModelCache>();

        // Saved state converter (serialises/deserialises ViewModel state from Android bundles)
        Services.TryAddSingleton<IMvxSavedStateConverter, MvxSavedStateConverter>();

        // Multiple ViewModel cache (for Fragment back-stack ViewModel reuse)
        Services.TryAddSingleton<IMvxMultipleViewModelCache, MvxMultipleViewModelCache>();

        // Android globals — provides ApplicationContext and executable assembly to binding components
        Services.TryAddSingleton<IMvxAndroidGlobals>(new MvxAndroidGlobals(application));

        // View presenter — uses an empty assembly list since view type registration
        // is handled explicitly via AddMvxAndroidView(s).
        // The activity lifetime listener is injected so PendingRequest handling works.
        Services.TryAddSingleton<IMvxAndroidViewPresenter>(sp =>
            new MvxAndroidViewPresenter(
                Array.Empty<Assembly>(),
                sp.GetService<IMvxAndroidActivityLifetimeListener>()));

        // IMvxViewPresenter is the platform-neutral interface used by navigation service
        Services.TryAddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxAndroidViewPresenter>());

        // View dispatcher (wraps the presenter for navigation service)
        Services.TryAddSingleton<IMvxViewDispatcher, MvxAndroidViewDispatcher>();

        // Views container (maps ViewModel types to Android Activity/Fragment types).
        // Factory applies all IMvxAndroidViewRegistration entries added via AddMvxAndroidView(s).
        Services.TryAddSingleton<MvxAndroidViewsContainer>(sp =>
        {
            var container = new MvxAndroidViewsContainer(
                sp.GetRequiredService<global::Android.Content.Context>());
            var typeFinder = sp.GetService<IMvxViewModelTypeFinder>();
            foreach (var reg in sp.GetServices<IMvxAndroidViewRegistration>())
                reg.Apply(container, typeFinder);
            return container;
        });
        // Register all interfaces that MvxAndroidViewsContainer implements
        Services.TryAddSingleton<IMvxAndroidViewsContainer>(
            sp => sp.GetRequiredService<MvxAndroidViewsContainer>());
        Services.TryAddSingleton<IMvxViewsContainer>(
            sp => sp.GetRequiredService<MvxAndroidViewsContainer>());
        Services.TryAddSingleton<IMvxAndroidViewModelRequestTranslator>(
            sp => sp.GetRequiredService<MvxAndroidViewsContainer>());
        Services.TryAddSingleton<IMvxAndroidViewModelLoader>(
            sp => sp.GetRequiredService<MvxAndroidViewsContainer>());
    }

    /// <summary>
    /// Replaces the default <see cref="IMvxAndroidViewPresenter"/> registration with a custom
    /// implementation resolved via dependency injection.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxAndroidViewPresenter"/>.</typeparam>
    /// <remarks>
    /// Also replaces the <see cref="IMvxViewPresenter"/> alias so navigation service and
    /// dispatcher both resolve the same instance. Call this before
    /// <see cref="MvxHostBuilder.StartWith{TViewModel}"/> or
    /// <see cref="MvxHostBuilder.ConfigureServices"/> to keep the platform-typed fluent chain.
    /// </remarks>
    public MvxAndroidHostBuilder UsePresenter<T>()
        where T : class, IMvxAndroidViewPresenter
    {
        Services.RemoveAll<IMvxAndroidViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxAndroidViewPresenter, T>();
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxAndroidViewPresenter>());
        return this;
    }

    /// <summary>
    /// Replaces the default <see cref="IMvxAndroidViewPresenter"/> registration with an instance
    /// produced by the supplied factory.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxAndroidViewPresenter"/>.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the presenter instance.</param>
    public MvxAndroidHostBuilder UsePresenter<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxAndroidViewPresenter
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<IMvxAndroidViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxAndroidViewPresenter>(factory);
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxAndroidViewPresenter>());
        return this;
    }

    /// <summary>
    /// Replaces the default <see cref="MvxAndroidViewsContainer"/> registration with a custom
    /// implementation resolved via dependency injection.
    /// </summary>
    /// <typeparam name="T">A type that implements both <see cref="IMvxAndroidViewsContainer"/>
    /// and <see cref="IMvxViewsContainer"/>.</typeparam>
    /// <remarks>
    /// Replaces all alias registrations so that <see cref="IMvxAndroidViewsContainer"/>,
    /// <see cref="IMvxViewsContainer"/>, <see cref="IMvxAndroidViewModelRequestTranslator"/>,
    /// and <see cref="IMvxAndroidViewModelLoader"/> all resolve to the same custom instance.
    /// Note that <see cref="IMvxAndroidViewsContainer"/> does not inherit from
    /// <see cref="IMvxViewsContainer"/> (separate interface hierarchies), hence both are
    /// required in the type constraint.
    /// </remarks>
    public MvxAndroidHostBuilder UseViewsContainer<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : class, IMvxAndroidViewsContainer, IMvxViewsContainer
    {
        Services.RemoveAll<MvxAndroidViewsContainer>();
        Services.RemoveAll<IMvxAndroidViewsContainer>();
        Services.RemoveAll<IMvxViewsContainer>();
        Services.RemoveAll<IMvxAndroidViewModelRequestTranslator>();
        Services.RemoveAll<IMvxAndroidViewModelLoader>();
        Services.AddSingleton<T>();
        Services.AddSingleton<IMvxAndroidViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxAndroidViewModelRequestTranslator>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxAndroidViewModelLoader>(sp => sp.GetRequiredService<T>());
        return this;
    }

    /// <summary>
    /// Replaces the default <see cref="MvxAndroidViewsContainer"/> registration with an instance
    /// produced by the supplied factory.
    /// </summary>
    /// <typeparam name="T">A type that implements both <see cref="IMvxAndroidViewsContainer"/>
    /// and <see cref="IMvxViewsContainer"/>.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the container instance.</param>
    public MvxAndroidHostBuilder UseViewsContainer<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxAndroidViewsContainer, IMvxViewsContainer
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<MvxAndroidViewsContainer>();
        Services.RemoveAll<IMvxAndroidViewsContainer>();
        Services.RemoveAll<IMvxViewsContainer>();
        Services.RemoveAll<IMvxAndroidViewModelRequestTranslator>();
        Services.RemoveAll<IMvxAndroidViewModelLoader>();
        Services.AddSingleton(factory);
        Services.AddSingleton<IMvxAndroidViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxViewsContainer>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxAndroidViewModelRequestTranslator>(sp => sp.GetRequiredService<T>());
        Services.AddSingleton<IMvxAndroidViewModelLoader>(sp => sp.GetRequiredService<T>());
        return this;
    }
}
