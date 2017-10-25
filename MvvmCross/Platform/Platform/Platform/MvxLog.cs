using System;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Platform.Platform
{
    public class MvxLog : IMvxLog, ILog
    {
        public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
        {
            Mvx.Trace(messageFunc.Invoke(), formatParameters);
            return true;
        }
    }
}
