// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Reflection;
using Android.App;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Base;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers;
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
    public static MvxAndroidHostBuilder CreateBuilder(Application application)
    {
        ArgumentNullException.ThrowIfNull(application);
        return new MvxAndroidHostBuilder(application);
    }

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
        // is handled explicitly via AddMvvmCrossAndroidView(s).
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
        // Factory applies all IMvxAndroidViewRegistration entries added via AddMvvmCrossAndroidView(s).
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

        // Register all binding services (value converters, binders, context stack, view type resolvers, etc.)
        // This is equivalent to what MvxAndroidSetup used to call via MvxAndroidBindingBuilder.
        var bindingBuilder = new MvxAndroidBindingBuilder(
            fillValueConverters: _ => { },
            fillValueCombiners: _ => { },
            fillTargetFactories: _ => { },
            fillBindingNames: _ => { },
            fillViewTypes: _ => { },
            fillAxmlViewTypeResolver: _ => { },
            fillNamespaceListViewTypeResolver: _ => { });
#pragma warning disable IL2026 // assembly-scanning trimming warning — acceptable at setup time
        bindingBuilder.DoRegistration(Services);
#pragma warning restore IL2026
    }
}
