// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Android.App;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Hosting;

namespace MvvmCross.Platforms.Android.Hosting;

/// <summary>
/// MvvmCross host builder for Android. Creates an <see cref="MvxHost"/> pre-wired with
/// Android platform context and services.
/// </summary>
public class MvxAndroidHostBuilder : MvxHostBuilder
{
    /// <summary>
    /// Creates an <see cref="MvxAndroidHostBuilder"/> for the given Android <see cref="Application"/>.
    /// The application instance is registered as a singleton so it can be resolved by services that
    /// need the Android context.
    /// </summary>
    public static MvxAndroidHostBuilder CreateBuilder(Application application)
    {
        ArgumentNullException.ThrowIfNull(application);
        return new MvxAndroidHostBuilder(application);
    }

    private MvxAndroidHostBuilder(Application application)
    {
        // Register the Android Application as a singleton for use in services that need Context.
        Services.TryAddSingleton(application);
        Services.TryAddSingleton<global::Android.Content.Context>(application);
    }
}
