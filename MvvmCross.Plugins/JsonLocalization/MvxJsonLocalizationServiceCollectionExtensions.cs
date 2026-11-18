// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Localization;

namespace MvvmCross.Plugin.JsonLocalization;

/// <summary>
/// Extension methods for registering the MvvmCross JsonLocalization plugin.
/// </summary>
public static class MvxJsonLocalizationServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="MvxJsonDictionaryTextProvider"/> services for JSON-based localisation.
    /// Users typically subclass <see cref="MvxTextProviderBuilder"/> and call
    /// <see cref="AddMvxJsonLocalization{TBuilder}"/> to register their builder.
    /// </summary>
    /// <typeparam name="TBuilder">
    /// A concrete subclass of <see cref="MvxTextProviderBuilder"/> that loads JSON resources.
    /// </typeparam>
    public static IServiceCollection AddMvxJsonLocalization<TBuilder>(
        this IServiceCollection services)
        where TBuilder : MvxTextProviderBuilder
    {
        services.TryAddSingleton<IMvxTextProviderBuilder, TBuilder>();
        services.TryAddSingleton<IMvxTextProvider>(
            sp => sp.GetRequiredService<IMvxTextProviderBuilder>().TextProvider);
        return services;
    }
}
