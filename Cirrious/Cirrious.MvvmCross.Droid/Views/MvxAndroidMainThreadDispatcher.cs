// MvxAndroidMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;
using Android.App;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidMainThreadDispatcher : MvxMainThreadDispatcher
    {
        public bool RequestMainThreadAction(Action action)
        {
            if (Application.SynchronizationContext == SynchronizationContext.Current)
                action();
            else
                Application.SynchronizationContext.Post(ignored => ExceptionMaskedAction(action), null);

            return true;
        }

        protected override bool IsInMainThread()
        {
            return Application.SynchronizationContext == SynchronizationContext.Current;
        }
    }
}