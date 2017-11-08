using System;
using MvvmCross.Platform.Logging;

namespace Playground.Uwp
{
    public class EmptyVoidLog : IMvxLog
    {
        public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
        {
            return true;
        }
    }
}