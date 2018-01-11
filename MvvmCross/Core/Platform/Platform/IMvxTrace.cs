// IMvxTrace.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Platform.Platform
{
    public interface IMvxTrace
    {
        void Trace(MvxTraceLevel level, string tag, Func<string> message);

        void Trace(MvxTraceLevel level, string tag, string message);

        void Trace(MvxTraceLevel level, string tag, string message, params object[] args);
    }
}