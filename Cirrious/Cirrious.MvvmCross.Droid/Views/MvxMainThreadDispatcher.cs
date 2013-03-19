// MvxMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Droid.Views
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