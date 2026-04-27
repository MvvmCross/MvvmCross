// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Tvos.Presenters;
using MvvmCross.Presenters;
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

    /// <summary>
    /// Registers a custom <see cref="IMvxTvosViewPresenter"/> implementation resolved via
    /// dependency injection, replacing any previously registered presenter.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxTvosViewPresenter"/>.</typeparam>
    /// <remarks>
    /// Also registers <see cref="IMvxViewPresenter"/> as an alias pointing to the same instance.
    /// Call this before <see cref="MvxHostBuilder.StartWith{TViewModel}"/> or
    /// <see cref="MvxHostBuilder.ConfigureServices"/> to keep the platform-typed fluent chain.
    /// </remarks>
    public MvxTvosHostBuilder UsePresenter<T>()
        where T : class, IMvxTvosViewPresenter
    {
        Services.RemoveAll<IMvxTvosViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxTvosViewPresenter, T>();
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxTvosViewPresenter>());
        return this;
    }

    /// <summary>
    /// Registers a custom <see cref="IMvxTvosViewPresenter"/> produced by the supplied factory,
    /// replacing any previously registered presenter.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxTvosViewPresenter"/>.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the presenter instance.</param>
    public MvxTvosHostBuilder UsePresenter<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxTvosViewPresenter
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<IMvxTvosViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxTvosViewPresenter>(factory);
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxTvosViewPresenter>());
        return this;
    }
}
