#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Mac.Views;

/// <summary>
/// Extension methods for registering macOS view-to-viewmodel mappings on an <see cref="IServiceCollection"/>.
/// </summary>
public static class MvxMacViewServiceCollectionExtensions
{
    /// <summary>
    /// Registers a single explicit View → ViewModel mapping.
    /// </summary>
    public static IServiceCollection AddMvxMacView<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] TView>(
        this IServiceCollection services)
        where TViewModel : IMvxViewModel
        where TView : IMvxView
    {
        services.AddSingleton<IMvxMacViewRegistration>(
            new MvxExplicitMacViewRegistration(typeof(TViewModel), typeof(TView)));
        return services;
    }

    /// <summary>
    /// Scans the given assembly for types that implement <see cref="IMvxView"/> and registers
    /// their ViewModel associations. Call this once per view assembly during startup.
    /// </summary>
    [RequiresUnreferencedCode("Assembly scanning for views uses reflection which may not be preserved during trimming. Use AddMvxMacView<TViewModel, TView>() for trim-compatible registration.")]
    public static IServiceCollection AddMvxMacViews(
        this IServiceCollection services,
        Assembly viewAssembly)
    {
        ArgumentNullException.ThrowIfNull(viewAssembly);
        services.AddSingleton<IMvxMacViewRegistration>(
            new MvxAssemblyScanMacViewRegistration(viewAssembly));
        return services;
    }
}

/// <summary>
/// Marker interface for deferred view registration applied when <see cref="MvxMacViewsContainer"/> is first built.
/// </summary>
internal interface IMvxMacViewRegistration
{
    void Apply(IMvxViewsContainer container, IMvxViewModelTypeFinder? typeFinder);
}

internal sealed class MvxExplicitMacViewRegistration(Type viewModelType, Type viewType)
    : IMvxMacViewRegistration
{
    public void Apply(IMvxViewsContainer container, IMvxViewModelTypeFinder? _)
        => container.Add(viewModelType, viewType);
}

internal sealed class MvxAssemblyScanMacViewRegistration(Assembly assembly)
    : IMvxMacViewRegistration
{
    [RequiresUnreferencedCode("Assembly scanning uses reflection")]
    public void Apply(IMvxViewsContainer container, IMvxViewModelTypeFinder? typeFinder)
    {
        if (typeFinder == null) return;

        foreach (var candidateViewType in assembly.ExceptionSafeGetTypes())
        {
            var viewModelType = typeFinder.FindTypeOrNull(candidateViewType);
            if (viewModelType != null)
                container.Add(viewModelType, candidateViewType);
        }
    }
}
