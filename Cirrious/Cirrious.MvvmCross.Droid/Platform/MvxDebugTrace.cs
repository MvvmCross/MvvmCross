// MvxDebugTrace.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Diagnostics;
using Android.Util;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Droid.Platform
{
    public class MvxDebugTrace : IMvxTrace
    {
        #region IMvxTrace Members

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
                Trace(MvxTraceLevel.Error, tag, "Exception during trace");
                Trace(level, tag, message);
            }
        }

        #endregion
    }
}