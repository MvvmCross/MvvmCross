// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Hosting;
using UIKit;

namespace MvvmCross.Platforms.Ios.Hosting;

/// <summary>
/// MvvmCross host builder for iOS/tvOS/MacCatalyst.
/// </summary>
public class MvxIosHostBuilder : MvxHostBuilder
{
    /// <summary>
    /// Creates an <see cref="MvxIosHostBuilder"/> with the given <see cref="UIWindow"/>.
    /// The window is registered as a singleton so the view presenter can resolve it.
    /// </summary>
    public static MvxIosHostBuilder CreateBuilder(UIWindow window)
    {
        ArgumentNullException.ThrowIfNull(window);
        return new MvxIosHostBuilder(window);
    }

    private MvxIosHostBuilder(UIWindow window)
    {
        Services.TryAddSingleton(window);
    }
}
