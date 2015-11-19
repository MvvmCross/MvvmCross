// MvxBindingTrace.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Binding
{
    public static class MvxBindingTrace
    {
        public static MvxTraceLevel TraceBindingLevel = MvxTraceLevel.Warning;

        public const string Tag = "MvxBind";

        public static void Trace(MvxTraceLevel level, string message, params object[] args)
        {
            if (level < TraceBindingLevel)
                return;

            MvxTrace.TaggedTrace(level, Tag, message, args);
        }

        public static void Trace(string message, params object[] args)
        {
            Trace(MvxTraceLevel.Diagnostic, message, args);
        }

        public static void Warning(string message, params object[] args)
        {
            Trace(MvxTraceLevel.Warning, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            Trace(MvxTraceLevel.Error, message, args);
        }
    }
}