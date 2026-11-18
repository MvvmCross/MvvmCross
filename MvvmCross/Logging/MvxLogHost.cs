#nullable enable
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvvmCross.Hosting;

namespace MvvmCross.Logging;

public static class MvxLogHost
{
    private static ILogger? _defaultLogger;

    public static ILogger? Default => _defaultLogger ??= GetLog("Default");

    public static ILogger<T>? GetLog<T>() =>
        MvxHost.Current?.Services.GetService<ILoggerFactory>()?.CreateLogger<T>();

    public static ILogger? GetLog(string categoryName) =>
        MvxHost.Current?.Services.GetService<ILoggerFactory>()?.CreateLogger(categoryName);
}
