// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.UI.Xaml;
using MvvmCross.Hosting;
using MvvmCross.Platforms.WinUi.Presenters;
using MvvmCross.Presenters;

namespace MvvmCross.Platforms.WinUi.Hosting;

/// <summary>
/// MvvmCross host builder for WinUI 3.
/// </summary>
public class MvxWinUiHostBuilder : MvxHostBuilder
{
    /// <summary>
    /// Creates an <see cref="MvxWinUiHostBuilder"/> with the given WinUI <see cref="Window"/>.
    /// </summary>
    public static MvxWinUiHostBuilder CreateBuilder(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);
        return new MvxWinUiHostBuilder(window);
    }

    private MvxWinUiHostBuilder(Window window)
    {
        Services.TryAddSingleton(window);
    }

    /// <summary>
    /// Registers a custom <see cref="IMvxWindowsViewPresenter"/> implementation resolved via
    /// dependency injection, replacing any previously registered presenter.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxWindowsViewPresenter"/>.</typeparam>
    /// <remarks>
    /// Also registers <see cref="IMvxViewPresenter"/> as an alias pointing to the same instance.
    /// Call this before <see cref="MvxHostBuilder.StartWith{TViewModel}"/> or
    /// <see cref="MvxHostBuilder.ConfigureServices"/> to keep the platform-typed fluent chain.
    /// </remarks>
    public MvxWinUiHostBuilder UsePresenter<T>()
        where T : class, IMvxWindowsViewPresenter
    {
        Services.RemoveAll<IMvxWindowsViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxWindowsViewPresenter, T>();
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxWindowsViewPresenter>());
        return this;
    }

    /// <summary>
    /// Registers a custom <see cref="IMvxWindowsViewPresenter"/> produced by the supplied factory,
    /// replacing any previously registered presenter.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxWindowsViewPresenter"/>.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the presenter instance.</param>
    public MvxWinUiHostBuilder UsePresenter<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxWindowsViewPresenter
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<IMvxWindowsViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxWindowsViewPresenter>(factory);
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxWindowsViewPresenter>());
        return this;
    }
}
