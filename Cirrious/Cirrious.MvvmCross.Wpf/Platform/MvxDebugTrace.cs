// MvxDebugTrace.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Wpf.Platform
{
    public class MvxDebugTrace : IMvxTrace
    {
        #region IMvxTrace Members

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            System.Diagnostics.Trace.WriteLine(tag + ": " + level + ": " + message);
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            System.Diagnostics.Trace.WriteLine(tag + ": " + level + ": " + string.Format(message, args));
        }

        #endregion
    }
}