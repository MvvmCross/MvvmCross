// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;

namespace MvvmCross.DependencyInjection;

/// <summary>
/// Optional application startup hook. Implement this to configure services and run code on
/// first launch, replacing the old MvxApplication base class.
/// Register via <see cref="MvxServiceCollectionExtensions.AddMvvmCross{TStartup}"/>.
/// </summary>
public interface IMvxStartup
{
    /// <summary>
    /// Called during host build to register application-level services.
    /// </summary>
    void ConfigureServices(IServiceCollection services);

    /// <summary>
    /// Called on the UI thread after the host is started. Use this to trigger first navigation.
    /// </summary>
    Task OnStartup(IServiceProvider services);
}
