// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
    public class MvxStopWatch
        : IDisposable
    {
        private static readonly IMvxLog _defaultLog = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor("mvxStopWatch");
        
        private readonly IMvxLog _log;
        private readonly string _message;
        private readonly int _startTickCount;

        private MvxStopWatch(string text, params object[] args)
        {
            _log = _defaultLog;
            _startTickCount = Environment.TickCount;
            _message = string.Format(text, args);
        }

        private MvxStopWatch(string tag, string text, params object[] args)
        {
            _log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor(tag);
            _startTickCount = Environment.TickCount;
            _message = string.Format(text, args);
        }

        public void Dispose()
        {
            MvxLog.Instance.Trace("{0} - {1}", Environment.TickCount - _startTickCount, _message);
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
}
