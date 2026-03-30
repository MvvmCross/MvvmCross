// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Hosting;
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
}
