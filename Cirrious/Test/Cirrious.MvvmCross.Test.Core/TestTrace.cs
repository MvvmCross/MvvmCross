using System.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Test.Core
{
    public class TestTrace : IMvxTrace
    {
        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            Debug.WriteLine(tag + ": " + level + ": " + message);
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            Debug.WriteLine(tag + ": " + level + ": " + message, args);
        }
    }
}