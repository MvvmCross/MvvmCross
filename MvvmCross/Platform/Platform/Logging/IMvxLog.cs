using System;

namespace MvvmCross.Platform.Logging
{
    public interface IMvxLog
    {
        bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters);
    }
}
