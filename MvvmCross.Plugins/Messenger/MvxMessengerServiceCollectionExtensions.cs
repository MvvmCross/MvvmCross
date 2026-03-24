// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MvvmCross.Plugin.Messenger;

/// <summary>
/// Extension methods for registering the MvvmCross Messenger plugin.
/// </summary>
public static class MvxMessengerServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IMvxMessenger"/> as a singleton using <see cref="MvxMessengerHub"/>.
    /// </summary>
    public static IServiceCollection AddMvvmCrossMessenger(this IServiceCollection services)
    {
        services.TryAddSingleton<IMvxMessenger, MvxMessengerHub>();
        return services;
    }
}
