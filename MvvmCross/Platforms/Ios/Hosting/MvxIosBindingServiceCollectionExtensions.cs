// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.Platforms.Ios.Binding;

namespace MvvmCross.Platforms.Ios.Hosting;

/// <summary>
/// Extension methods for registering the MvvmCross binding engine for iOS/MacCatalyst.
/// </summary>
public static class MvxIosBindingServiceCollectionExtensions
{
    /// <summary>
    /// Registers the MvvmCross data binding engine for iOS/MacCatalyst.
    /// Call this inside <c>ConfigureServices</c> to opt into binding support.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Optional callback to customize binding registrations.</param>
    [RequiresUnreferencedCode("Registering iOS binding services uses reflection which may not be preserved during trimming.")]
    public static IServiceCollection AddMvxBindings(
        this IServiceCollection services,
        Action<MvxIosBindingConfiguration>? configure = null)
    {
        var config = new MvxIosBindingConfiguration();
        configure?.Invoke(config);
        var builder = new MvxIosBindingBuilder(
            fillRegistryAction: config._fillTargetFactories,
            fillValueConvertersAction: config._fillValueConverters,
            fillValueCombinersAction: config._fillValueCombiners,
            fillBindingNamesAction: config._fillBindingNames);
        builder.DoRegistration(services);
        return services;
    }
}

/// <summary>
/// Configuration object for the iOS MvvmCross binding engine.
/// Passed to the callback in <see cref="MvxIosBindingServiceCollectionExtensions.AddMvxBindings"/>.
/// </summary>
public sealed class MvxIosBindingConfiguration
{
    internal Action<IMvxTargetBindingFactoryRegistry> _fillTargetFactories = _ => { };
    internal Action<IMvxValueConverterRegistry> _fillValueConverters = _ => { };
    internal Action<IMvxValueCombinerRegistry> _fillValueCombiners = _ => { };
    internal Action<IMvxBindingNameRegistry> _fillBindingNames = _ => { };

    /// <summary>Registers custom target binding factories.</summary>
    public MvxIosBindingConfiguration FillTargetFactories(Action<IMvxTargetBindingFactoryRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = _fillTargetFactories;
        _fillTargetFactories = r => { previous(r); configure(r); };
        return this;
    }

    /// <summary>Registers additional value converters.</summary>
    public MvxIosBindingConfiguration FillValueConverters(Action<IMvxValueConverterRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = _fillValueConverters;
        _fillValueConverters = r => { previous(r); configure(r); };
        return this;
    }

    /// <summary>Registers additional value combiners.</summary>
    public MvxIosBindingConfiguration FillValueCombiners(Action<IMvxValueCombinerRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = _fillValueCombiners;
        _fillValueCombiners = r => { previous(r); configure(r); };
        return this;
    }

    /// <summary>Registers additional default binding names.</summary>
    public MvxIosBindingConfiguration FillBindingNames(Action<IMvxBindingNameRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = _fillBindingNames;
        _fillBindingNames = r => { previous(r); configure(r); };
        return this;
    }
}
