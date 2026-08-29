// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers;

namespace MvvmCross.Platforms.Android.Binding;

/// <summary>
/// Extension methods for registering Android view types with MvvmCross binding.
/// </summary>
public static class MvxAndroidBindingServiceCollectionExtensions
{
    /// <summary>
    /// Registers a custom Android <see cref="View"/> type with the MvvmCross view type registry
    /// so it can be resolved by short name during XML binding inflation.
    /// </summary>
    /// <typeparam name="TView">The Android <see cref="View"/> type to register.</typeparam>
    /// <remarks>
    /// This replaces the old assembly-scanning behaviour. Call this for every custom view type
    /// that you reference by short name in MvvmCross binding XML (e.g. <c>MvxLinearLayout</c>).
    /// </remarks>
    public static IServiceCollection AddMvxAndroidViewType<TView>(
        this IServiceCollection services)
        where TView : View
    {
        // Use a post-registration callback so the registry singleton is populated
        // after it has been created by the binding builder.
        services.AddSingleton<IMvxViewTypeRegistration>(
            new MvxViewTypeRegistration(typeof(TView)));
        return services;
    }
}

/// <summary>
/// Deferred view type registration that is applied to <see cref="IMvxViewTypeRegistry"/>
/// once the DI container is built.
/// </summary>
internal interface IMvxViewTypeRegistration
{
    void Apply(IMvxViewTypeRegistry registry);
}

internal sealed class MvxViewTypeRegistration : IMvxViewTypeRegistration
{
    private readonly Type _viewType;

    public MvxViewTypeRegistration(Type viewType)
    {
        _viewType = viewType;
    }

    public void Apply(IMvxViewTypeRegistry registry) => registry.Register(_viewType);
}
