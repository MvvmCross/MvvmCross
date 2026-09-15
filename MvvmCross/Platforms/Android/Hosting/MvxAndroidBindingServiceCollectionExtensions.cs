// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers;

namespace MvvmCross.Platforms.Android.Hosting;

/// <summary>
/// Extension methods for registering the MvvmCross binding engine for Android.
/// </summary>
public static class MvxAndroidBindingServiceCollectionExtensions
{
    /// <summary>
    /// Registers the MvvmCross data binding engine for Android.
    /// Call this inside <c>ConfigureServices</c> to opt into binding support.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Optional callback to customize binding registrations.</param>
    [RequiresUnreferencedCode("Registering Android binding services uses reflection which may not be preserved during trimming.")]
    public static IServiceCollection AddMvxBindings(
        this IServiceCollection services,
        Action<MvxAndroidBindingConfiguration>? configure = null)
    {
        var config = new MvxAndroidBindingConfiguration();
        configure?.Invoke(config);
        var builder = new MvxAndroidBindingBuilder(
            fillValueConverters: config._fillValueConverters,
            fillValueCombiners: config._fillValueCombiners,
            fillTargetFactories: config._fillTargetFactories,
            fillBindingNames: config._fillBindingNames,
            fillViewTypes: config._fillViewTypes,
            fillAxmlViewTypeResolver: config._fillAxmlViewTypeResolver,
            fillNamespaceListViewTypeResolver: config._fillNamespaceListViewTypeResolver);
        builder.DoRegistration(services);
        return services;
    }
}

/// <summary>
/// Configuration object for the Android MvvmCross binding engine.
/// Passed to the callback in <see cref="MvxAndroidBindingServiceCollectionExtensions.AddMvxBindings"/>.
/// </summary>
public sealed class MvxAndroidBindingConfiguration
{
    internal Action<IMvxTargetBindingFactoryRegistry> _fillTargetFactories = _ => { };
    internal Action<IMvxValueConverterRegistry> _fillValueConverters = _ => { };
    internal Action<IMvxValueCombinerRegistry> _fillValueCombiners = _ => { };
    internal Action<IMvxBindingNameRegistry> _fillBindingNames = _ => { };
    internal Action<IMvxViewTypeRegistry> _fillViewTypes = _ => { };
    internal Action<IMvxAxmlNameViewTypeResolver> _fillAxmlViewTypeResolver = _ => { };
    internal Action<IMvxNamespaceListViewTypeResolver> _fillNamespaceListViewTypeResolver = _ => { };

    /// <summary>Registers custom target binding factories.</summary>
    public MvxAndroidBindingConfiguration FillTargetFactories(Action<IMvxTargetBindingFactoryRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = _fillTargetFactories;
        _fillTargetFactories = r => { previous(r); configure(r); };
        return this;
    }

    /// <summary>Registers additional value converters.</summary>
    public MvxAndroidBindingConfiguration FillValueConverters(Action<IMvxValueConverterRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = _fillValueConverters;
        _fillValueConverters = r => { previous(r); configure(r); };
        return this;
    }

    /// <summary>Registers additional value combiners.</summary>
    public MvxAndroidBindingConfiguration FillValueCombiners(Action<IMvxValueCombinerRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = _fillValueCombiners;
        _fillValueCombiners = r => { previous(r); configure(r); };
        return this;
    }

    /// <summary>Registers additional default binding names.</summary>
    public MvxAndroidBindingConfiguration FillBindingNames(Action<IMvxBindingNameRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = _fillBindingNames;
        _fillBindingNames = r => { previous(r); configure(r); };
        return this;
    }

    /// <summary>Registers additional Android view types for the XML inflater.</summary>
    public MvxAndroidBindingConfiguration FillViewTypes(Action<IMvxViewTypeRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = _fillViewTypes;
        _fillViewTypes = r => { previous(r); configure(r); };
        return this;
    }

    /// <summary>Configures the AXML view type resolver for Android XML layout binding.</summary>
    public MvxAndroidBindingConfiguration FillAxmlViewTypeResolver(Action<IMvxAxmlNameViewTypeResolver> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = _fillAxmlViewTypeResolver;
        _fillAxmlViewTypeResolver = r => { previous(r); configure(r); };
        return this;
    }

    /// <summary>Configures the namespace list view type resolver for Android view resolution.</summary>
    public MvxAndroidBindingConfiguration FillNamespaceListViewTypeResolver(Action<IMvxNamespaceListViewTypeResolver> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = _fillNamespaceListViewTypeResolver;
        _fillNamespaceListViewTypeResolver = r => { previous(r); configure(r); };
        return this;
    }
}
