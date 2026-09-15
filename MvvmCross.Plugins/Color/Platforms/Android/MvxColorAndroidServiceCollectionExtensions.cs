// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Color.Platforms.Android;

/// <summary>
/// Extension methods for registering the MvvmCross Color plugin on Android.
/// </summary>
public static class MvxColorAndroidServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="MvxAndroidColor"/> as <see cref="IMvxNativeColor"/> and
    /// adds the common Color value converters.
    /// </summary>
    public static IServiceCollection AddMvxColor(this IServiceCollection services)
    {
        services.TryAddSingleton<IMvxNativeColor, MvxAndroidColor>();
        services.AddMvxColorConverters();
        return services;
    }
}
