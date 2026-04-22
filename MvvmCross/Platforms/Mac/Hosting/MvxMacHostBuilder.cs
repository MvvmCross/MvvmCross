// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using AppKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Mac.Presenters;
using MvvmCross.Presenters;

namespace MvvmCross.Platforms.Mac.Hosting;

/// <summary>
/// MvvmCross host builder for macOS.
/// </summary>
public class MvxMacHostBuilder : MvxHostBuilder
{
    /// <summary>
    /// Creates an <see cref="MvxMacHostBuilder"/> with the given <see cref="NSWindow"/>.
    /// </summary>
    public static MvxMacHostBuilder CreateBuilder(NSWindow window)
    {
        ArgumentNullException.ThrowIfNull(window);
        return new MvxMacHostBuilder(window);
    }

    private MvxMacHostBuilder(NSWindow window)
    {
        Services.TryAddSingleton(window);
    }

    /// <summary>
    /// Registers a custom <see cref="IMvxMacViewPresenter"/> implementation resolved via
    /// dependency injection, replacing any previously registered presenter.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxMacViewPresenter"/>.</typeparam>
    /// <remarks>
    /// Also registers <see cref="IMvxViewPresenter"/> as an alias pointing to the same instance.
    /// Call this before <see cref="MvxHostBuilder.StartWith{TViewModel}"/> or
    /// <see cref="MvxHostBuilder.ConfigureServices"/> to keep the platform-typed fluent chain.
    /// </remarks>
    public MvxMacHostBuilder UsePresenter<T>()
        where T : class, IMvxMacViewPresenter
    {
        Services.RemoveAll<IMvxMacViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxMacViewPresenter, T>();
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxMacViewPresenter>());
        return this;
    }

    /// <summary>
    /// Registers a custom <see cref="IMvxMacViewPresenter"/> produced by the supplied factory,
    /// replacing any previously registered presenter.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxMacViewPresenter"/>.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the presenter instance.</param>
    public MvxMacHostBuilder UsePresenter<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxMacViewPresenter
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<IMvxMacViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxMacViewPresenter>(factory);
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxMacViewPresenter>());
        return this;
    }
}
