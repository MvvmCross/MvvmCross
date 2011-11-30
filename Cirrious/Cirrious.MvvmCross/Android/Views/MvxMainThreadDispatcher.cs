#region Copyright

// <copyright file="MvxMainThreadDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

#region using

using System;
using Android.App;
using Cirrious.MvvmCross.Interfaces.Views;

#endregion

namespace Cirrious.MvvmCross.Android.Views
{
    public class MvxMainThreadDispatcher : IMvxMainThreadDispatcher
    {
        private readonly Activity _activity;
        private bool _stopRequested = false;

        public MvxMainThreadDispatcher(Activity activity)
        {
            _activity = activity;
        }

        public bool RequestMainThreadAction(Action action)
        {
            return InvokeOrBeginInvoke(action);
        }

#warning RequestStop removed at present
        //public void RequestStop()
        //{
        //    _stopRequested = true;
        //}

        protected bool InvokeOrBeginInvoke(Action action)
        {
            if (_stopRequested)
                return false;

            _activity.RunOnUiThread(action);

            return true;
        }
    }
}