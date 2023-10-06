// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.Binding
{
#nullable enable
    public static class MvxBindingLog
    {
        public static ILogger? Instance { get; } = MvxLogHost.GetLog("MvxBind");

        public static LogLevel TraceBindingLevel { get; set; } = LogLevel.Warning;

        private static void Trace(LogLevel level, string message, params object[] args)
        {
            if ((int)level < (int)TraceBindingLevel) return;

            Instance?.Log(level, message, args);
        }

        public static void Trace(string message, params object[] args)
        {
            Trace(LogLevel.Trace, message, args);
        }

        public static void Warning(string message, params object[] args)
        {
            Trace(LogLevel.Warning, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            Trace(LogLevel.Error, message, args);
        }
    }
#nullable restore
}
