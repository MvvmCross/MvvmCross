using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Platform.Logging;

namespace Playground.Forms.Uwp
{

    public class EmptyVoidLog : IMvxLog
    {
        public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
        {
            return true;
        }
    }

    public class EmptyVoidLogProvider : IMvxLogProvider
    {
        private EmptyVoidLog _voidLog;

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
