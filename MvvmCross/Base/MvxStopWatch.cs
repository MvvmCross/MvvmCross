// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
#nullable enable
    public sealed class MvxStopWatch
        : IDisposable
    {
        private readonly IMvxLog? _log;
        private readonly string _message;
        private readonly int _startTickCount;

        private MvxStopWatch(string text, params object[] args)
        {
            _log = GetLog(nameof(MvxStopWatch));
            _startTickCount = Environment.TickCount;
            _message = string.Format(text, args);
        }

        private MvxStopWatch(string tag, string text, params object[] args)
        {
            _log = GetLog(tag);
            _startTickCount = Environment.TickCount;
            _message = string.Format(text, args);
        }

        private static IMvxLog? GetLog(string tag)
        {
            if (Mvx.IoCProvider.TryResolve(out IMvxLogProvider logProvider))
            {
                return logProvider.GetLogFor(tag);
            }

            return null;
        }

        public void Dispose()
        {
            _log?.Trace("{0} - {1}", Environment.TickCount - _startTickCount, _message);
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
