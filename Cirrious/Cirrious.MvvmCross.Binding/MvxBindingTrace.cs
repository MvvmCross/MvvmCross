using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding
{
    public static class MvxBindingTrace
    {
        public static MvxBindingTraceLevel Level = MvxBindingTraceLevel.Warning;

        public const string Tag = "MvxBind";

        public static void Trace(MvxBindingTraceLevel level, string message, params object[] args)
        {
            if (level >= Level)
                MvxTrace.TaggedTrace(Tag, message, args);
        }
    }
}