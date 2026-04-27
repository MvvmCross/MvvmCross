// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Views;

/// <summary>
/// Extension methods for registering Android view-to-viewmodel mappings on an <see cref="IServiceCollection"/>.
/// </summary>
public static class MvxAndroidViewServiceCollectionExtensions
{
    /// <summary>
    /// Registers a single explicit View → ViewModel mapping.
    /// </summary>
    public static IServiceCollection AddMvxAndroidView<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] TView>(
        this IServiceCollection services)
        where TViewModel : IMvxViewModel
        where TView : IMvxView
    {
        services.AddSingleton<IMvxAndroidViewRegistration>(
            new MvxExplicitAndroidViewRegistration(typeof(TViewModel), typeof(TView)));
        return services;
    }

    /// <summary>
    /// Scans the given assembly for types that implement <see cref="IMvxView"/> and registers
    /// their ViewModel associations using <see cref="IMvxViewModelTypeFinder"/> conventions
    /// (attribute-based or name-based mapping). Call this once per view assembly during startup.
    /// </summary>
    [RequiresUnreferencedCode("Assembly scanning for views uses reflection which may not be preserved during trimming. Use AddMvxAndroidView<TViewModel, TView>() for trim-compatible registration.")]
    public static IServiceCollection AddMvxAndroidViews(
        this IServiceCollection services,
        Assembly viewAssembly)
    {
        ArgumentNullException.ThrowIfNull(viewAssembly);
        services.AddSingleton<IMvxAndroidViewRegistration>(
            new MvxAssemblyScanAndroidViewRegistration(viewAssembly));
        return services;
    }
}

/// <summary>
/// Marker interface for deferred view registration applied when <see cref="MvxAndroidViewsContainer"/> is first built.
/// </summary>
internal interface IMvxAndroidViewRegistration
{
    [RequiresUnreferencedCode("Applying Android view registrations may use reflection for assembly scanning.")]
    void Apply(IMvxViewsContainer container, IMvxViewModelTypeFinder? typeFinder);
}

internal sealed class MvxExplicitAndroidViewRegistration(
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] Type viewType)
    : IMvxAndroidViewRegistration
{
    [RequiresUnreferencedCode("Applying explicit Android view registrations uses reflection via IMvxViewsContainer.Add.")]
    public void Apply(IMvxViewsContainer container, IMvxViewModelTypeFinder? _)
        => container.Add(viewModelType, viewType);
}

internal sealed class MvxAssemblyScanAndroidViewRegistration(Assembly assembly)
    : IMvxAndroidViewRegistration
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
