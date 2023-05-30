#nullable enable
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;

namespace MvvmCross.Hosting;

public sealed class MvxHost : IMvxHost
{
    private static IMvxHost? _currentHost;

    public static IMvxHost Current
    {
        get
        {
            if (_currentHost == null)
                throw new InvalidOperationException("MvxHost has not been created yet. This means MvvmCross hasn't been initialized properly.");

            return _currentHost;
        }
        private set => _currentHost = value;
    }

    public static bool IsInitialized => _currentHost != null;
    public static IServiceProvider ServiceProvider => Current.Services;
    public static ILoggerFactory LoggingFactory => Current.Logging;
    public static T? GetService<T>() => ServiceProvider.GetService<T>();

    public IServiceProvider Services { get; init; }
    public ILoggerFactory Logging { get; init; }

    public MvxHost(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        Services = serviceProvider;
        Logging = loggerFactory;
    }

    public void Run()
    {
        Current = this;
    }

    public void Dispose()
    {
        Services.DisposeIfDisposable();
        Logging.Dispose();
    }
}
