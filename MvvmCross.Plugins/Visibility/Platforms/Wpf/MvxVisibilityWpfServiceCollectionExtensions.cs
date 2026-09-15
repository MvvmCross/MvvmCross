// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.UI;

namespace MvvmCross.Plugin.Visibility.Platforms.Wpf;

public static class MvxVisibilityWpfServiceCollectionExtensions
{
    public static IServiceCollection AddMvxVisibility(this IServiceCollection services)
    {
        services.TryAddSingleton<IMvxNativeVisibility, MvxWpfVisibility>();
        services.AddMvxVisibilityConverters();
        return services;
    }
}
