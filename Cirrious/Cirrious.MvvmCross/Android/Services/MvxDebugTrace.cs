#region Copyright

// <copyright file="MvxDebugTrace.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Diagnostics;
using Cirrious.MvvmCross.Interfaces.Services;

namespace Cirrious.MvvmCross.Android.Services
{
    public class MvxDebugTrace : IMvxTrace
    {
        #region IMvxTrace Members

        public void Trace(string tag, string message)
        {
            global::Android.Util.Log.Info(tag, message);
            Debug.WriteLine(tag + ":" + message);
        }

        public void Trace(string tag, string message, params object[] args)
        {
            try
            {
                global::Android.Util.Log.Info(tag, message, args);
                Debug.WriteLine(string.Format(tag + ":" + message, args));
            }
            catch (FormatException)
            {
                Trace(tag, "Exception during trace");
                Trace(tag, message);
            }
        }

        #endregion
    }
}