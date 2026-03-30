// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using AppKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Hosting;

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
}
