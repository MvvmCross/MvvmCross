using System;
using MvvmCross.Platform.Logging;

namespace Playground.Uwp
{
    public class EmptyVoidLogProvider : IMvxLogProvider
    {
        private readonly EmptyVoidLog _voidLog;

        public EmptyVoidLogProvider()
        {
            _voidLog = new EmptyVoidLog();
        }

        public IMvxLog GetLogFor<T>()
        {
            return _voidLog;
        }

        public IMvxLog GetLogFor(string name)
        {
            return _voidLog;
        }

        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }

        public IDisposable OpenMappedContext(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
