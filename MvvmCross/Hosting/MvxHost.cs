// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvvmCross.DependencyInjection;
using MvvmCross.Logging;
using MvvmCross.ViewModels;

namespace MvvmCross.Hosting;

/// <summary>
/// Represents the running MvvmCross application host, built by a platform-specific
/// <see cref="MvxHostBuilder"/>. Exposes <see cref="Services"/> for use as a service
/// locator from contexts that cannot participate in constructor injection (e.g. platform
/// application entry points, XAML markup extensions).
/// </summary>
public class MvxHost
{
    private static MvxHost? _current;

    /// <summary>
    /// The currently running host. Set by <see cref="Start"/>. Null before the host is started.
    /// </summary>
    public static MvxHost? Current => _current;

    /// <summary>
    /// The built service provider. Use this for service location when constructor injection
    /// is not possible. Framework-internal code must use constructor injection instead.
    /// </summary>
    public IServiceProvider Services { get; }

    internal MvxHost(IServiceProvider services)
    {
        Services = services;
    }

    /// <summary>
    /// Sets this host as the current ambient host, then triggers <see cref="IMvxStartup.OnStartup"/>
    /// (if registered) on the calling (UI) thread.
    /// </summary>
    public virtual async Task Start()
    {
        _current = this;

        var startup = Services.GetService<IMvxStartup>();
        if (startup is not null)
        {
            await startup.OnStartup(Services).ConfigureAwait(false);
            return;
        }

        // Fall back to IMvxAppStart if no IMvxStartup was registered.
        var appStart = Services.GetService<IMvxAppStart>();
        if (appStart is not null)
        {
            await appStart.Start(null).ConfigureAwait(false);
        }
        else
        {
            Services.GetService<ILogger<MvxHost>>()
                ?.Log(LogLevel.Warning,
                    "MvxHost started but no IMvxStartup or IMvxAppStart was registered. " +
                    "Register one via AddMvvmCross(options => options.StartWith<TViewModel>()) " +
                    "or implement IMvxStartup.");
        }
    }

    /// <summary>
    /// Sets a pre-built service provider as the current host. For use in unit tests only.
    /// </summary>
    internal static void InitializeForTesting(IServiceProvider serviceProvider)
    {
        _current = new MvxHost(serviceProvider);
    }

    /// <summary>
    /// Clears the static <see cref="Current"/> property. For use in unit tests only.
    /// </summary>
    internal static void ResetForTesting()
    {
        _current = null;
    }
}
