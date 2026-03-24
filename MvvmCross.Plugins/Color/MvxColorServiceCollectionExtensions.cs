// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Converters;

namespace MvvmCross.Plugin.Color;

/// <summary>
/// Extension methods for registering the MvvmCross Color plugin — platform-neutral services.
/// For the platform-specific <c>IMvxNativeColor</c> registration, call the platform extension
/// (e.g. <c>AddMvvmCrossColor()</c> from the platform-specific package).
/// </summary>
public static class MvxColorServiceCollectionExtensions
{
    /// <summary>
    /// Registers the common Color plugin value converters (ARGB, RGBA, RGB, NativeColor, etc.)
    /// into <see cref="IMvxValueConverterRegistry"/>.
    /// Call this from your platform-specific <c>AddMvvmCrossColor()</c> extension or directly
    /// before calling the platform overload.
    /// </summary>
    public static IServiceCollection AddMvvmCrossColorConverters(this IServiceCollection services)
    {
        services.AddSingleton<IConfigureMvxValueConverters, MvxColorValueConverterRegistration>();
        return services;
    }

    private sealed class MvxColorValueConverterRegistration : IConfigureMvxValueConverters
    {
        public void Register(IMvxValueConverterRegistry registry)
        {
            registry.AddOrOverwrite("ARGB", new MvxARGBValueConverter());
            registry.AddOrOverwrite("NativeColor", new MvxNativeColorValueConverter());
            registry.AddOrOverwrite("RGBA", new MvxRGBAValueConverter());
            registry.AddOrOverwrite("RGB", new MvxRGBValueConverter());
            registry.AddOrOverwrite("RGBIntColor", new MvxRGBIntColorValueConverter());
        }
    }
}
