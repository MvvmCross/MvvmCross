// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Visibility.Platforms.Android;

public static class MvxVisibilityAndroidServiceCollectionExtensions
{
    public static IServiceCollection AddMvvmCrossVisibility(this IServiceCollection services)
    {
        services.TryAddSingleton<IMvxNativeVisibility, MvxDroidVisibility>();
        services.AddMvvmCrossVisibilityConverters();
        return services;
    }
}
