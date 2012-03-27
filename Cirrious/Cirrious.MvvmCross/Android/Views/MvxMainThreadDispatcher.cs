#region Copyright
// <copyright file="MvxMainThreadDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using Android.App;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;

#endregion

namespace Cirrious.MvvmCross.Android.Views
{
    public class MvxMainThreadDispatcher : IMvxMainThreadDispatcher
    {
        private readonly Activity _activity;

        public MvxMainThreadDispatcher(Activity activity)
        {
            _activity = activity;
        }

        #region IMvxMainThreadDispatcher Members

        public bool RequestMainThreadAction(Action action)
        {
            return InvokeOrBeginInvoke(action);
        }

        #endregion

        private bool InvokeOrBeginInvoke(Action action)
        {
            if (_activity == null)
            {
                MvxTrace.Trace("Warning - UI action being ignored - no current activity");
                return false;
            }
            _activity.RunOnUiThread(action);

            return true;
        }
    }
}