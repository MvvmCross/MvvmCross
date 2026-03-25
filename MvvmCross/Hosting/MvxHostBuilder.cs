// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.DependencyInjection;

namespace MvvmCross.Hosting;

/// <summary>
/// Abstract base for platform-specific MvvmCross host builders.
/// Platform-specific subclasses (e.g. <c>MvxAndroidHostBuilder</c>) provide
/// a static <c>CreateBuilder</c> factory method that accepts platform context.
/// </summary>
public abstract class MvxHostBuilder
{
    /// <summary>
    /// The service collection used to register framework and application services.
    /// Platform host builders pre-register platform-specific services before
    /// handing control to <see cref="ConfigureServices"/>.
    /// </summary>
    public IServiceCollection Services { get; } = new ServiceCollection();

    // Binding fill callbacks — populated by Configure* methods, consumed in Build().
    protected Action<IMvxTargetBindingFactoryRegistry> FillTargetFactories { get; private set; } = _ => { };
    protected Action<IMvxValueConverterRegistry> FillValueConverters { get; private set; } = _ => { };
    protected Action<IMvxValueCombinerRegistry> FillValueCombiners { get; private set; } = _ => { };
    protected Action<IMvxBindingNameRegistry> FillBindingNames { get; private set; } = _ => { };

    /// <summary>
    /// Adds services to the container using the provided callback.
    /// </summary>
    public MvxHostBuilder ConfigureServices(Action<IServiceCollection> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        configure(Services);
        return this;
    }

    /// <summary>
    /// Registers custom target binding factories
    /// (equivalent to overriding <c>FillTargetFactories</c> in the old setup classes).
    /// </summary>
    public MvxHostBuilder ConfigureTargetBindings(Action<IMvxTargetBindingFactoryRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = FillTargetFactories;
        FillTargetFactories = registry => { previous(registry); configure(registry); };
        return this;
    }

    /// <summary>
    /// Registers additional value converters
    /// (equivalent to overriding <c>FillValueConverters</c> in the old setup classes).
    /// </summary>
    public MvxHostBuilder ConfigureValueConverters(Action<IMvxValueConverterRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = FillValueConverters;
        FillValueConverters = registry => { previous(registry); configure(registry); };
        return this;
    }

    /// <summary>
    /// Registers additional value combiners
    /// (equivalent to overriding <c>FillValueCombiners</c> in the old setup classes).
    /// </summary>
    public MvxHostBuilder ConfigureValueCombiners(Action<IMvxValueCombinerRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = FillValueCombiners;
        FillValueCombiners = registry => { previous(registry); configure(registry); };
        return this;
    }

    /// <summary>
    /// Registers additional default binding names
    /// (equivalent to overriding <c>FillDefaultBindingNames</c> in the old setup classes).
    /// </summary>
    public MvxHostBuilder ConfigureBindingNames(Action<IMvxBindingNameRegistry> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        var previous = FillBindingNames;
        FillBindingNames = registry => { previous(registry); configure(registry); };
        return this;
    }

    /// <summary>
    /// Builds the <see cref="MvxHost"/> by finalising the service registrations
    /// and building the <see cref="IServiceProvider"/>.
    /// </summary>
    public virtual MvxHost Build()
    {
        var provider = Services.BuildServiceProvider();
        return CreateHost(provider);
    }

    /// <summary>
    /// Factory method called by <see cref="Build"/> to create the host instance.
    /// Override to return a platform-specific <see cref="MvxHost"/> subclass.
    /// </summary>
    protected virtual MvxHost CreateHost(IServiceProvider serviceProvider)
        => new MvxHost(serviceProvider);
}

