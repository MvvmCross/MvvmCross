using System;
using System.Diagnostics;
using MvvmCross.Platform.Platform;

namespace Playground.Forms.Droid
{
    public class DebugTrace : IMvxTrace
    {
        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            Debug.WriteLine($"{tag} : {level} : {message()}");
        }

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            Debug.WriteLine($"{tag} : {level} : {message}");
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            try
            {
                Debug.WriteLine($"{tag} : {level} : {message}, {args}");
            }
            catch (FormatException)
            {
                Trace(MvxTraceLevel.Error, tag, "Exception during trace of {0} {1}", level, message);
            }
        }
    }
}
