// MvxAutoViewTrace.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.AutoView
{
    public static class MvxAutoViewTrace
    {
        public static MvxTraceLevel TraceBindingLevel = MvxTraceLevel.Warning;

        public const string Tag = "MvxAutoView";

        public static void Trace(MvxTraceLevel level, string message, params object[] args)
        {
            if (level < TraceBindingLevel)
                return;

            MvxTrace.TaggedTrace(level, Tag, message, args);
        }
    }
}