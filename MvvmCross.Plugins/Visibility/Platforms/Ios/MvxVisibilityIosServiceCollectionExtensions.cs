// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Visibility.Platforms.Ios;

public static class MvxVisibilityIosServiceCollectionExtensions
{
    public static IServiceCollection AddMvvmCrossVisibility(this IServiceCollection services)
    {
        services.TryAddSingleton<IMvxNativeVisibility, MvxIosVisibility>();
        services.AddMvvmCrossVisibilityConverters();
        return services;
    }
}
