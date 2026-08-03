// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.Presenters;
using System.Windows;

namespace MvvmCross.Platforms.Wpf.Hosting;

/// <summary>
/// MvvmCross host builder for WPF.
/// </summary>
public class MvxWpfHostBuilder : MvxHostBuilder
{
    /// <summary>
    /// Creates an <see cref="MvxWpfHostBuilder"/> with the given WPF <see cref="Window"/>.
    /// The window is registered as a singleton so the view presenter can resolve it.
    /// </summary>
    public static MvxWpfHostBuilder CreateBuilder(Window mainWindow)
    {
        ArgumentNullException.ThrowIfNull(mainWindow);
        return new MvxWpfHostBuilder(mainWindow);
    }

    private MvxWpfHostBuilder(Window mainWindow)
    {
        Services.TryAddSingleton(mainWindow);
    }

    /// <summary>
    /// Registers a custom <see cref="IMvxWpfViewPresenter"/> implementation resolved via
    /// dependency injection, replacing any previously registered presenter.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxWpfViewPresenter"/>.</typeparam>
    /// <remarks>
    /// Also registers <see cref="IMvxViewPresenter"/> as an alias pointing to the same instance.
    /// Call this before <see cref="MvxHostBuilder.StartWith{TViewModel}"/> or
    /// <see cref="MvxHostBuilder.ConfigureServices"/> to keep the platform-typed fluent chain.
    /// </remarks>
    public MvxWpfHostBuilder UsePresenter<T>()
        where T : class, IMvxWpfViewPresenter
    {
        Services.RemoveAll<IMvxWpfViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxWpfViewPresenter, T>();
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxWpfViewPresenter>());
        return this;
    }

    /// <summary>
    /// Registers a custom <see cref="IMvxWpfViewPresenter"/> produced by the supplied factory,
    /// replacing any previously registered presenter.
    /// </summary>
    /// <typeparam name="T">A type that implements <see cref="IMvxWpfViewPresenter"/>.</typeparam>
    /// <param name="factory">Factory delegate that receives the <see cref="IServiceProvider"/>
    /// and returns the presenter instance.</param>
    public MvxWpfHostBuilder UsePresenter<T>(Func<IServiceProvider, T> factory)
        where T : class, IMvxWpfViewPresenter
    {
        ArgumentNullException.ThrowIfNull(factory);
        Services.RemoveAll<IMvxWpfViewPresenter>();
        Services.RemoveAll<IMvxViewPresenter>();
        Services.AddSingleton<IMvxWpfViewPresenter>(factory);
        Services.AddSingleton<IMvxViewPresenter>(
            sp => sp.GetRequiredService<IMvxWpfViewPresenter>());
        return this;
    }
}
