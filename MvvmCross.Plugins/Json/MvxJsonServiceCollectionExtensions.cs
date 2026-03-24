// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Base;

namespace MvvmCross.Plugin.Json;

/// <summary>
/// Extension methods for registering the MvvmCross Json plugin.
/// </summary>
public static class MvxJsonServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IMvxJsonConverter"/> and optionally <see cref="IMvxTextSerializer"/>
    /// using <see cref="MvxJsonConverter"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="registerAsTextSerializer">
    /// When <c>true</c>, also registers <see cref="MvxJsonConverter"/> as <see cref="IMvxTextSerializer"/>.
    /// </param>
    [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
    public static IServiceCollection AddMvvmCrossJson(
        this IServiceCollection services,
        bool registerAsTextSerializer = false)
    {
        services.TryAddSingleton<IMvxJsonConverter, MvxJsonConverter>();

        if (registerAsTextSerializer)
            services.TryAddSingleton<IMvxTextSerializer, MvxJsonConverter>();

        return services;
    }
}
