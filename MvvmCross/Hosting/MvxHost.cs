// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using System.Diagnostics.CodeAnalysis;

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
    public static MvxHost? Current => _current ?? TryBuildFromTestAccessor();

    /// <summary>
    /// When set, provides a fallback <see cref="IServiceProvider"/> for test environments where
    /// <see cref="InitializeForTesting"/> has not yet been called. The accessor may lazily build
    /// the provider on first call. For use by test infrastructure only.
    /// </summary>
    public static Func<IServiceProvider?>? TestServiceProviderAccessor { get; set; }

    private static MvxHost? TryBuildFromTestAccessor()
    {
        if (TestServiceProviderAccessor == null) return null;
        var provider = TestServiceProviderAccessor();
        return provider != null ? new MvxHost(provider) : null;
    }

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
    /// Sets this host as the current ambient host. On platforms with a dedicated start screen
    /// (e.g. Android's <c>MvxStartActivity</c>) navigation to the first ViewModel is handled
    /// there; on headless or desktop platforms (WPF, console) <see cref="IMvxAppStart"/> is
    /// triggered here.
    /// </summary>
    [RequiresUnreferencedCode("Navigation uses presentation attributes and view type lookups that may not be preserved during trimming.")]
    public virtual async Task Start()
    {
        _current = this;

        var appStart = Services.GetService<IMvxAppStart>();
        if (appStart is not null)
        {
            await appStart.StartAsync(null).ConfigureAwait(false);
        }
        else
        {
            Services.GetService<ILogger<MvxHost>>()
                ?.Log(LogLevel.Warning,
                    "MvxHost started but no IMvxAppStart was registered. " +
                    "Call builder.StartWith<TViewModel>() to configure the initial navigation.");
        }
    }

    /// <summary>
    /// Sets this instance as <see cref="Current"/>. Called by <see cref="Start"/>; exposed
    /// as a protected helper for platform-specific subclasses that override <see cref="Start"/>.
    /// </summary>
    protected void SetAsCurrent() => _current = this;

    /// <summary>
    /// Sets a pre-built service provider as the current host. For use in unit tests only.
    /// </summary>
    public static void InitializeForTesting(IServiceProvider serviceProvider)
    {
        _current = new MvxHost(serviceProvider);
    }

    /// <summary>
    /// Clears the static <see cref="Current"/> property and <see cref="TestServiceProviderAccessor"/>.
    /// For use in unit tests only.
    /// </summary>
    public static void ResetForTesting()
    {
        _current = null;
        TestServiceProviderAccessor = null;
    }
}
