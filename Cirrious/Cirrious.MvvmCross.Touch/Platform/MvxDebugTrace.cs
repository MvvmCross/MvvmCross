// MvxDebugTrace.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Touch.Platform
{
    public class MvxDebugTrace : IMvxTrace
    {
        #region IMvxTrace Members

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
#if DEBUG
			Console.WriteLine(tag + ": " + level + ": " + message);
#endif
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
#if DEBUG
			Console.WriteLine(tag + ": " + level + ": " + message, args);
#endif
        }

        #endregion
    }
}