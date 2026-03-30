// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.DependencyInjection;
using MvvmCross.ViewModels;

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

    /// <summary>
    /// Registers the MvvmCross framework and configures <typeparamref name="TViewModel"/> as the
    /// first ViewModel to navigate to on startup.
    /// </summary>
    /// <typeparam name="TViewModel">The initial ViewModel the app should navigate to.</typeparam>
    /// <param name="configure">Optional callback for additional options such as <see cref="MvxOptions.AddViewAssembly"/>.</param>
    [RequiresUnreferencedCode("Configuring MvvmCross via StartWith<TViewModel>() stores types that are registered via reflection.")]
    public MvxHostBuilder StartWith<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>(
        Action<MvxOptions>? configure = null)
            where TViewModel : IMvxViewModel
    {
        Services.AddMvvmCross(opts =>
        {
            opts.StartWith<TViewModel>();
            configure?.Invoke(opts);
        });
        return this;
    }

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

