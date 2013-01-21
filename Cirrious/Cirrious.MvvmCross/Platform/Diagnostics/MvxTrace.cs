// MvxTrace.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Core;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Platform.Diagnostics
{
    public class MvxTrace
        : MvxSingleton<IMvxTrace>
        , IMvxTrace
        , IMvxServiceConsumer
    {
        #region public static Interface

        public static readonly DateTime WhenTraceStartedUtc = DateTime.UtcNow;

        public static string DefaultTag { get; set; }

        public static void Initialize()
        {
            if (Instance != null)
                throw new MvxException("MvxTrace already initialized");

            DefaultTag = "mvx";
            var selfRegisteringSingleton = new MvxTrace();
        }

        public static void TaggedTrace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            if (Instance != null)
                Instance.Trace(level, tag, PrependWithTime(message), args);
        }

        public static void Trace(MvxTraceLevel level, string message, params object[] args)
        {
            if (Instance != null)
                Instance.Trace(level, DefaultTag, PrependWithTime(message), args);
        }

        public static void TaggedTrace(string tag, string message, params object[] args)
        {
            TaggedTrace(MvxTraceLevel.Diagnostic, tag, message, args);
        }

        public static void Trace(string message, params object[] args)
        {
            Trace(MvxTraceLevel.Diagnostic, message, args);
        }

        #endregion Static Interface

        private readonly IMvxTrace _realTrace;

        public MvxTrace()
        {
			_realTrace = this.GetService<IMvxTrace>();
            if (_realTrace == null)
                throw new MvxException("No platform trace service available");
        }

        #region IMvxTrace Members

        void IMvxTrace.Trace(MvxTraceLevel level, string tag, string message)
        {
            _realTrace.Trace(level, tag, message);
        }

        void IMvxTrace.Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            _realTrace.Trace(level, tag, message, args);
        }

        #endregion

        #region private helpers

        private static string PrependWithTime(string input)
        {
            var timeIntoApp = (DateTime.UtcNow - WhenTraceStartedUtc).TotalSeconds;
            return string.Format("{0,6:0.00} {1}", timeIntoApp, input);
        }

        #endregion
    }
}