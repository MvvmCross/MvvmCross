// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Localization;

namespace MvvmCross.Plugin.ResxLocalization;

/// <summary>
/// Extension methods for registering the MvvmCross ResxLocalization plugin.
/// </summary>
public static class MvxResxLocalizationServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="MvxResxTextProvider"/> as <see cref="IMvxTextProvider"/> using the
    /// supplied <paramref name="resourceManager"/>.
    /// </summary>
    public static IServiceCollection AddMvxResxLocalization(
        this IServiceCollection services,
        ResourceManager resourceManager)
    {
        services.TryAddSingleton<IMvxTextProvider>(
            _ => new MvxResxTextProvider(resourceManager));
        return services;
    }

    /// <summary>
    /// Registers <see cref="MvxResxTextProvider"/> as <see cref="IMvxTextProvider"/> using the
    /// supplied list of <paramref name="resourceManagers"/>.
    /// </summary>
    public static IServiceCollection AddMvxResxLocalization(
        this IServiceCollection services,
        IList<ResourceManager> resourceManagers)
    {
        services.TryAddSingleton<IMvxTextProvider>(
            _ => new MvxResxTextProvider(resourceManagers));
        return services;
    }
}
