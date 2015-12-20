// MvxTrace.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Platform
{
    using System;

    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Exceptions;

    public class MvxTrace
        : MvxSingleton<IMvxTrace>
          , IMvxTrace

    {
        #region public static Interface

        public static readonly DateTime WhenTraceStartedUtc = DateTime.UtcNow;

        public static string DefaultTag { get; set; }

        public static void Initialize()
        {
            if (Instance != null)
                throw new MvxException("MvxTrace already initialized");

            DefaultTag = "mvx";
            // selfRegisteringSingleton
            new MvxTrace();
        }

        public static void TaggedTrace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            Instance?.Trace(level, tag, PrependWithTime(message), args);
        }

        public static void TaggedTrace(MvxTraceLevel level, string tag, Func<string> message)
        {
            Instance?.Trace(level, tag, PrependWithTime(message));
        }

        public static void Trace(MvxTraceLevel level, string message, params object[] args)
        {
            Instance?.Trace(level, DefaultTag, PrependWithTime(message), args);
        }

        public static void Trace(MvxTraceLevel level, Func<string> message)
        {
            Instance?.Trace(level, DefaultTag, PrependWithTime(message));
        }

        public static void TaggedTrace(string tag, string message, params object[] args)
        {
            TaggedTrace(MvxTraceLevel.Diagnostic, tag, message, args);
        }

        public static void TaggedWarning(string tag, string message, params object[] args)
        {
            TaggedTrace(MvxTraceLevel.Warning, tag, message, args);
        }

        public static void TaggedError(string tag, string message, params object[] args)
        {
            TaggedTrace(MvxTraceLevel.Error, tag, message, args);
        }

        public static void TaggedTrace(string tag, Func<string> message)
        {
            TaggedTrace(MvxTraceLevel.Diagnostic, tag, message);
        }

        public static void TaggedWarning(string tag, Func<string> message)
        {
            TaggedTrace(MvxTraceLevel.Warning, tag, message);
        }

        public static void TaggedError(string tag, Func<string> message)
        {
            TaggedTrace(MvxTraceLevel.Error, tag, message);
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

        public static void Trace(Func<string> message)
        {
            Trace(MvxTraceLevel.Diagnostic, message);
        }

        public static void Warning(Func<string> message)
        {
            Trace(MvxTraceLevel.Warning, message);
        }

        public static void Error(Func<string> message)
        {
            Trace(MvxTraceLevel.Error, message);
        }

        #endregion public static Interface

        private readonly IMvxTrace _realTrace;

        public MvxTrace()
        {
            this._realTrace = Mvx.Resolve<IMvxTrace>();
            if (this._realTrace == null)
                throw new MvxException("No platform trace service available");
        }

        #region IMvxTrace Members

        void IMvxTrace.Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            this._realTrace.Trace(level, tag, message);
        }

        void IMvxTrace.Trace(MvxTraceLevel level, string tag, string message)
        {
            this._realTrace.Trace(level, tag, message);
        }

        void IMvxTrace.Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            this._realTrace.Trace(level, tag, message, args);
        }

        #endregion IMvxTrace Members

        #region private helpers

        private static string PrependWithTime(string input)
        {
            var timeIntoApp = (DateTime.UtcNow - WhenTraceStartedUtc).TotalSeconds;
            return $"{timeIntoApp,6:0.00} {input}";
        }

        private static Func<string> PrependWithTime(Func<string> input)
        {
            return () =>
                {
                    var result = input();
                    return PrependWithTime(result);
                };
        }

        #endregion private helpers
    }
}