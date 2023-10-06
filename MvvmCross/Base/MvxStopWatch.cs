// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
#nullable enable
    public sealed class MvxStopWatch
        : IDisposable
    {
        private readonly ILogger? _log;
        private readonly string _message;
        private readonly int _startTickCount;

        private MvxStopWatch(string text, params object[] args)
        {
            _log = MvxLogHost.GetLog<MvxStopWatch>();
            _startTickCount = Environment.TickCount;
            _message = string.Format(text, args);
        }

        private MvxStopWatch(string tag, string text, params object[] args)
        {
            _log = MvxLogHost.GetLog(tag);
            _startTickCount = Environment.TickCount;
            _message = string.Format(text, args);
        }

        public void Dispose()
        {
            _log?.Log(LogLevel.Trace, "{Ticks} - {Message}", Environment.TickCount - _startTickCount, _message);
            GC.SuppressFinalize(this);
        }

        public static MvxStopWatch Create(string text, params object[] args)
        {
            return new MvxStopWatch(text, args);
        }

        public static MvxStopWatch CreateWithTag(string tag, string text, params object[] args)
        {
            return new MvxStopWatch(tag, text, args);
        }
    }
#nullable restore
}
