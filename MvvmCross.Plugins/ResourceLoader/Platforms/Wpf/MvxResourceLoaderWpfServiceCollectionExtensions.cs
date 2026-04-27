// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Base;

namespace MvvmCross.Plugin.ResourceLoader.Platforms.Wpf;

public static class MvxResourceLoaderWpfServiceCollectionExtensions
{
    public static IServiceCollection AddMvxResourceLoader(this IServiceCollection services)
    {
        services.TryAddTransient<IMvxResourceLoader, MvxWpfResourceLoader>();
        return services;
    }
}
