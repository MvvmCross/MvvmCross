// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Logging;

namespace MvvmCross.Binding
{
    public static class MvxBindingLog
    {
        public static IMvxLog Instance { get; } = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor("MvxBind");

        public static MvxLogLevel TraceBindingLevel = MvxLogLevel.Warn;

        private static void Trace(MvxLogLevel level, string message, params object[] args)
        {
            if (level < TraceBindingLevel) return;

            Instance.Log(level, () => string.Format(message, args));
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
