// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Binding;
using MvvmCross.Converters;

namespace MvvmCross.Plugin.Visibility;

/// <summary>
/// Extension methods for registering the MvvmCross Visibility plugin — platform-neutral.
/// </summary>
public static class MvxVisibilityServiceCollectionExtensions
{
    /// <summary>
    /// Registers common Visibility value converters into <see cref="IMvxValueConverterRegistry"/>.
    /// </summary>
    public static IServiceCollection AddMvvmCrossVisibilityConverters(this IServiceCollection services)
    {
        services.AddSingleton<IConfigureMvxValueConverters, MvxVisibilityValueConverterRegistration>();
        return services;
    }

    private sealed class MvxVisibilityValueConverterRegistration : IConfigureMvxValueConverters
    {
        public void Register(IMvxValueConverterRegistry registry)
        {
            registry.AddOrOverwrite("Visibility", new MvxVisibilityValueConverter());
            registry.AddOrOverwrite("InvertedVisibility", new MvxInvertedVisibilityValueConverter());
        }
    }
}
