// MvxDebugTrace.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Platform
{
    using System;
    using System.Diagnostics;

    using Android.Util;

    using MvvmCross.Platform.Platform;

    public class MvxDebugTrace : IMvxTrace
    {
        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            this.Trace(level, tag, message());
        }

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            Log.Info(tag, message);
            Debug.WriteLine(tag + ":" + level + ":" + message);
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            try
            {
                Log.Info(tag, message, args);
                Debug.WriteLine(string.Format(tag + ":" + level + ":" + message, args));
            }
            catch (FormatException)
            {
                this.Trace(MvxTraceLevel.Error, tag, "Exception during trace");
                this.Trace(level, tag, message);
            }
        }
    }
}