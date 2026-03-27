// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Color.Platforms.Wpf;

/// <summary>
/// Extension methods for registering the MvvmCross Color plugin on WPF.
/// </summary>
public static class MvxColorWpfServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="MvxWpfColor"/> as <see cref="IMvxNativeColor"/> and
    /// adds the common Color value converters.
    /// </summary>
    public static IServiceCollection AddMvxColor(this IServiceCollection services)
    {
        services.TryAddSingleton<IMvxNativeColor, MvxWpfColor>();
        services.AddMvxColorConverters();
        return services;
    }
}
