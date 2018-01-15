using System;

using MvvmCross.Platform.Platform;

namespace MvvmCross.Mac.Platform
{
    public class MvxDebugTrace : IMvxTrace
    {
        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            Console.WriteLine(tag + ":" + level + ":" + message());
        }

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            Console.WriteLine(tag + ": " + level + ": " + message);
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            Console.WriteLine(tag + ": " + level + ": " + message, args);
        }
    }
}