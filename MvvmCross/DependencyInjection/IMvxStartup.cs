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
    /// Called on the UI thread after the host is started.
    /// Use this for service-level initialisation (registering additional services, etc.).
    /// <para>
    /// On Android, do NOT trigger navigation here — the platform's <c>MvxStartActivity</c>
    /// handles first navigation via <c>IMvxAppStart</c>. On WPF/Console platforms without a
    /// dedicated start screen you may navigate here.
    /// </para>
    /// </summary>
    Task OnStartup(IServiceProvider services);
}
