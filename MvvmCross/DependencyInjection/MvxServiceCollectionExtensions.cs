// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Commands;
using MvvmCross.Core;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCross.ViewModels.Result;

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
    [RequiresUnreferencedCode("Configuring MvvmCross via StartWith<TViewModel>() or UseAppStart<TAppStart>() stores types that are registered via reflection. Use explicit type parameters for trim-compatible setup.")]
    public static IServiceCollection AddMvxCore(
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
    /// Scans the given assembly for <see cref="IMvxViewModel"/> implementations and registers
    /// them by name so they can be looked up by <see cref="IMvxViewModelByNameLookup"/>.
    /// Call this for each assembly that contains ViewModels (typically your Core assembly).
    /// </summary>
    [RequiresUnreferencedCode("Assembly scanning for ViewModels uses reflection which may not be preserved during trimming. Register ViewModels explicitly with IMvxViewModelByNameRegistry.Add<T>() for trim-compatible registration.")]
    public static IServiceCollection AddMvxViewModels(
        this IServiceCollection services,
        Assembly viewModelAssembly)
    {
        ArgumentNullException.ThrowIfNull(viewModelAssembly);
        services.AddSingleton<IMvxViewModelRegistration>(
            new MvxAssemblyViewModelRegistration(viewModelAssembly));
        return services;
    }

    [RequiresUnreferencedCode("Registering ViewModel lookup factory uses reflection via IMvxViewModelRegistration.Apply which may scan assemblies.")]
    private static void RegisterCoreServices(IServiceCollection services)
    {
        // Ensure ILoggerFactory and ILogger<T> are always resolvable with a no-op (silent) implementation
        // by default. Framework classes such as MvxNavigationViewModel require ILoggerFactory as a
        // non-nullable constructor parameter, so DI resolution would throw without this.
        // AddLogging() is idempotent: a subsequent call such as
        //   services.AddLogging(b => b.AddSerilog())
        // simply adds providers to the already-registered factory rather than replacing it.
        services.AddLogging();

        // Settings
        services.TryAddSingleton<IMvxSettings, MvxSettings>();

        // String / type parsing (navigation parameter serialisation)
        services.TryAddSingleton<IMvxStringToTypeParser, MvxStringToTypeParser>();
        services.TryAddSingleton<IMvxFillableStringToTypeParser>(
            sp => (IMvxFillableStringToTypeParser)sp.GetRequiredService<IMvxStringToTypeParser>());

        // Navigation serialiser
        services.TryAddSingleton<IMvxNavigationSerializer, MvxStringDictionaryNavigationSerializer>();

        // ViewModel name lookup (shared instance registered under two interfaces)
        // Factory applies all IMvxViewModelRegistration entries added via AddMvxViewModels().
        services.TryAddSingleton<MvxViewModelByNameLookup>(sp =>
        {
            var lookup = new MvxViewModelByNameLookup();
            foreach (var reg in sp.GetServices<IMvxViewModelRegistration>())
                reg.Apply(lookup);
            return lookup;
        });
        services.TryAddSingleton<IMvxViewModelByNameLookup>(
            sp => sp.GetRequiredService<MvxViewModelByNameLookup>());
        services.TryAddSingleton<IMvxViewModelByNameRegistry>(
            sp => sp.GetRequiredService<MvxViewModelByNameLookup>());

        // ViewModel type finding
        services.TryAddSingleton<IMvxNameMapping>(
            _ => new MvxPostfixAwareViewToViewModelNameMapping("View", "Activity", "Fragment", "Page", "Controller"));
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

    [RequiresUnreferencedCode("Registering IMvxAppStart via Type may use reflection. Use UseAppStart<TAppStart>() with a concrete type for trim-compatible registration.")]
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

/// <summary>Marker interface for deferred ViewModel name registry population.</summary>
internal interface IMvxViewModelRegistration
{
    [RequiresUnreferencedCode("Applying ViewModel registrations may use reflection for assembly scanning.")]
    void Apply(IMvxViewModelByNameRegistry registry);
}

internal sealed class MvxAssemblyViewModelRegistration(System.Reflection.Assembly assembly)
    : IMvxViewModelRegistration
{
    [RequiresUnreferencedCode("Assembly scanning uses reflection")]
    public void Apply(IMvxViewModelByNameRegistry registry) => registry.AddAll(assembly);
}
