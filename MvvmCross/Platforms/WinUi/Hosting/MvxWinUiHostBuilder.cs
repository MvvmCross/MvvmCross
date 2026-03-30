// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.UI.Xaml;
using MvvmCross.Hosting;

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
}
