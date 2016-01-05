// MvxTraceTrace.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Wpf.Platform
{
    using System;

    using MvvmCross.Platform.Platform;

    public class MvxTraceTrace : IMvxTrace
    {
        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            System.Diagnostics.Trace.WriteLine(tag + ":" + level + ":" + message());
        }

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            System.Diagnostics.Trace.WriteLine(tag + ": " + level + ": " + message);
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            System.Diagnostics.Trace.WriteLine(string.Format(tag + ": " + level + ": " + message, args));
        }
    }
}