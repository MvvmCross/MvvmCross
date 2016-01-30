// MvxDebugTrace.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Apple.iOS.Platform
{
    using System;
    using System.Diagnostics;
    using MvvmCross.Platform.Platform;

    public class MvxDebugTrace : IMvxTrace
    {
        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            Debug.WriteLine($"{tag}:{level}:{message()}");
        }

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            Debug.WriteLine($"{tag}:{level}:{message}");
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            Debug.WriteLine(tag + ": " + level + ": " + message, args);
        }
    }
}