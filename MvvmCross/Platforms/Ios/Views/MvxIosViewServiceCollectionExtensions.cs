// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Ios.Views;

/// <summary>
/// Extension methods for registering iOS view-to-viewmodel mappings on an <see cref="IServiceCollection"/>.
/// </summary>
public static class MvxIosViewServiceCollectionExtensions
{
    /// <summary>
    /// Registers a single explicit View → ViewModel mapping.
    /// </summary>
    public static IServiceCollection AddMvxIosView<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.Interfaces)] TView>(
        this IServiceCollection services)
        where TViewModel : IMvxViewModel
        where TView : IMvxView
    {
        services.AddSingleton<IMvxIosViewRegistration>(
            new MvxExplicitIosViewRegistration(typeof(TViewModel), typeof(TView)));
        return services;
    }

    /// <summary>
    /// Scans the given assembly for types that implement <see cref="IMvxView"/> and registers
    /// their ViewModel associations. Call this once per view assembly during startup.
    /// </summary>
    [RequiresUnreferencedCode("Assembly scanning for views uses reflection which may not be preserved during trimming. Use AddMvxIosView<TViewModel, TView>() for trim-compatible registration.")]
    public static IServiceCollection AddMvxIosViews(
        this IServiceCollection services,
        Assembly viewAssembly)
    {
        ArgumentNullException.ThrowIfNull(viewAssembly);
        services.AddSingleton<IMvxIosViewRegistration>(
            new MvxAssemblyScanIosViewRegistration(viewAssembly));
        return services;
    }
}

/// <summary>
/// Marker interface for deferred view registration applied when <see cref="MvxIosViewsContainer"/> is first built.
/// </summary>
internal interface IMvxIosViewRegistration
{
    void Apply(IMvxViewsContainer container, IMvxViewModelTypeFinder? typeFinder);
}

internal sealed class MvxExplicitIosViewRegistration(Type viewModelType, Type viewType)
    : IMvxIosViewRegistration
{
    public void Apply(IMvxViewsContainer container, IMvxViewModelTypeFinder? _)
        => container.Add(viewModelType, viewType);
}

internal sealed class MvxAssemblyScanIosViewRegistration(Assembly assembly)
    : IMvxIosViewRegistration
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
