// MvxBindingTrace.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Binding
{
    public static class MvxBindingLog
    {
        public static IMvxLog Log = Mvx.Resolve<IMvxLogProvider>().GetLogFor("MvxBind");

        public static MvxLogLevel TraceBindingLevel = MvxLogLevel.Warn;

        private static void Trace(MvxLogLevel level, string message, params object[] args)
        {
            if (level < TraceBindingLevel) return;

            Log.Log(level, () => string.Format(message, args));
        }

        public static void Trace(string message, params object[] args)
        {
            Trace(MvxLogLevel.Trace, message, args);
        }

        public static void Warning(string message, params object[] args)
        {
            Trace(MvxLogLevel.Warn, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            Trace(MvxLogLevel.Error, message, args);
        }
    }
}
