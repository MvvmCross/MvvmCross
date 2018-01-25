// MvxStopWatch.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Platform.Platform
{
    public class MvxStopWatch
        : IDisposable
    {
        private static readonly IMvxLog _defaultLog = Mvx.Resolve<IMvxLogProvider>().GetLogFor("mvxStopWatch");
        
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
            _log = Mvx.Resolve<IMvxLogProvider>().GetLogFor(tag);
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