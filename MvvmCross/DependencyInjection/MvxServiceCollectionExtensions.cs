// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Commands;
using MvvmCross.Core;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCross.ViewModels.Result;
using MvvmCross.Views;

namespace MvvmCross.DependencyInjection;

/// <summary>
/// Extension methods for registering the MvvmCross framework with <see cref="IServiceCollection"/>.
/// </summary>
public static class MvxServiceCollectionExtensions
{
    /// <summary>
    /// Registers all core MvvmCross framework services. Call once per application host.
    /// </summary>
    /// <param name="services">The service collection to add MvvmCross services to.</param>
    /// <param name="configure">Optional callback to customise framework options.</param>
    public static IServiceCollection AddMvvmCross(
        this IServiceCollection services,
        Action<MvxOptions>? configure = null)
    {
        var options = new MvxOptions();
        configure?.Invoke(options);

        RegisterCoreServices(services);
        RegisterAppStart(services, options);

        return services;
    }

    /// <summary>
    /// Registers all core MvvmCross framework services and registers a user-defined
    /// <see cref="IMvxStartup"/> implementation.
    /// </summary>
    public static IServiceCollection AddMvvmCross<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TStartup>(
        this IServiceCollection services,
        Action<MvxOptions>? configure = null)
            where TStartup : class, IMvxStartup
    {
        services.AddMvvmCross(configure);
        services.TryAddSingleton<IMvxStartup, TStartup>();
        return services;
    }

    private static void RegisterCoreServices(IServiceCollection services)
    {
        // Settings
        services.TryAddSingleton<IMvxSettings, MvxSettings>();

        // String / type parsing (navigation parameter serialisation)
        services.TryAddSingleton<IMvxStringToTypeParser, MvxStringToTypeParser>();
        services.TryAddSingleton<IMvxFillableStringToTypeParser>(
            sp => (IMvxFillableStringToTypeParser)sp.GetRequiredService<IMvxStringToTypeParser>());

        // Navigation serialiser
        services.TryAddSingleton<IMvxNavigationSerializer, MvxStringDictionaryNavigationSerializer>();

        // ViewModel name lookup (shared instance registered under two interfaces)
        services.TryAddSingleton<MvxViewModelByNameLookup>();
        services.TryAddSingleton<IMvxViewModelByNameLookup>(
            sp => sp.GetRequiredService<MvxViewModelByNameLookup>());
        services.TryAddSingleton<IMvxViewModelByNameRegistry>(
            sp => sp.GetRequiredService<MvxViewModelByNameLookup>());

        // ViewModel type finding
        services.TryAddSingleton<IMvxViewModelTypeFinder, MvxViewModelViewTypeFinder>();
        services.TryAddSingleton<IMvxTypeToTypeLookupBuilder, MvxViewModelViewLookupBuilder>();

        // ViewModel loading
        services.TryAddSingleton<IMvxViewModelLoader, MvxViewModelLoader>();
        services.TryAddSingleton<IMvxViewModelLocator, MvxDefaultViewModelLocator>();

        // Navigation service
        services.TryAddSingleton<IMvxNavigationService, MvxNavigationService>();

        // Result ViewModel manager
        services.TryAddSingleton<IMvxResultViewModelManager, MvxResultViewModelManager>();

        // Child ViewModel cache
        services.TryAddSingleton<IMvxChildViewModelCache, MvxChildViewModelCache>();

        // Commands
        services.TryAddSingleton<IMvxCommandCollectionBuilder, MvxCommandCollectionBuilder>();
        services.TryAddTransient<IMvxCommandHelper, MvxWeakCommandHelper>();

        // ViewModelLocatorCollection — default implementation provided; platform host builders
        // register their own IMvxViewsContainer which typically also implements this interface.
        services.TryAddSingleton<IMvxViewModelLocatorCollection, MvxViewModelLocatorCollection>();
    }

    private static void RegisterAppStart(IServiceCollection services, MvxOptions options)
    {
        if (options.AppStartType != null)
        {
            services.TryAddSingleton(typeof(IMvxAppStart), options.AppStartType);
        }
        else if (options.AppStartViewModelType != null)
        {
            var vmType = options.AppStartViewModelType;
            var appStartType = typeof(MvxAppStart<>).MakeGenericType(vmType);
            services.TryAddSingleton(typeof(IMvxAppStart), appStartType);
        }
    }
}
