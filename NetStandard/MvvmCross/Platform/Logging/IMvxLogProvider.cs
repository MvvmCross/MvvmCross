using System;

namespace MvvmCross.Platform.Logging
{
    public interface IMvxLogProvider
    {
        IMvxLog GetLogFor<T>();

        IMvxLog GetLogFor(string name);

        IDisposable OpenNestedContext(string message);

        IDisposable OpenMappedContext(string key, string value);
    }
}
