using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.Conference.Core
{
    public class Trace
    {
        private const string Tag = "Conference";

        public static void Info(string message, params object[] args)
        {
            MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, Tag, message, args);    
        }

        public static void Warn(string message, params object[] args)
        {
            MvxTrace.TaggedTrace(MvxTraceLevel.Warning, Tag, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            MvxTrace.TaggedTrace(MvxTraceLevel.Error, Tag, message, args);
        }
    }
}