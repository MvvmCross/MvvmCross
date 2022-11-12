using Microsoft.Extensions.Logging;

namespace MvvmCross.Logging
{
#nullable enable
    public static class MvxLogHost
    {
        private static ILogger? _defaultLogger;

        public static ILogger? Default
        {
            get
            {
                return _defaultLogger ??= GetLog("Default");
            }
        }

        public static ILogger<T>? GetLog<T>()
        {
            if (Mvx.IoCProvider.TryResolve<ILoggerFactory>(out var loggerFactory))
            {
                return loggerFactory.CreateLogger<T>();
            }

            return null;
        }

        public static ILogger? GetLog(string categoryName)
        {
            if (Mvx.IoCProvider.TryResolve<ILoggerFactory>(out var loggerFactory))
            {
                return loggerFactory.CreateLogger(categoryName);
            }

            return null;
        }
    }
#nullable restore
}
