// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Tvos.Binding;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Hosting;

/// <summary>
/// MvvmCross host builder for tvOS.
/// </summary>
public class MvxTvosHostBuilder : MvxHostBuilder
{
    /// <summary>
    /// Creates an <see cref="MvxTvosHostBuilder"/> with the given <see cref="UIWindow"/>.
    /// </summary>
    public static MvxTvosHostBuilder CreateBuilder(UIWindow window)
    {
        ArgumentNullException.ThrowIfNull(window);
        return new MvxTvosHostBuilder(window);
    }

    private MvxTvosHostBuilder(UIWindow window)
    {
        Services.TryAddSingleton(window);
    }

    /// <inheritdoc/>
    public override MvxHost Build()
    {
        var bindingBuilder = new MvxTvosBindingBuilder(
            fillRegistryAction: FillTargetFactories,
            fillValueConvertersAction: FillValueConverters,
            fillBindingNamesAction: FillBindingNames,
            fillValueCombinersAction: FillValueCombiners);
#pragma warning disable IL2026
        bindingBuilder.DoRegistration(Services);
#pragma warning restore IL2026
        return base.Build();
    }
}
