// MvxStopWatch.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Platform
{
    public class MvxStopWatch
        : IDisposable
    {
        private readonly string _message;
        private readonly int _startTickCount;
        private readonly string _tag;

        private MvxStopWatch(string tag, string text, params object[] args)
        {
            _tag = tag;
            _startTickCount = Environment.TickCount;
            _message = string.Format(text, args);
        }

        public void Dispose()
        {
            MvxTrace.TaggedTrace(_tag, "{0} - {1}", Environment.TickCount - _startTickCount, _message);
            GC.SuppressFinalize(this);
        }

        public static MvxStopWatch CreateWithTag(string tag, string text, params object[] args)
        {
            return new MvxStopWatch(tag, text, args);
        }

        public static MvxStopWatch Create(string text, params object[] args)
        {
            return CreateWithTag("mvxStopWatch", text, args);
        }
    }
}