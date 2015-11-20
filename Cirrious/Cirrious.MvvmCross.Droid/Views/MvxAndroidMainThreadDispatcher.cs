// MvxAndroidMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using System;
using System.Threading;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class MvxAndroidMainThreadDispatcher : MvxMainThreadDispatcher
    {
        public bool RequestMainThreadAction(Action action)
        {
            if (Android.App.Application.SynchronizationContext == SynchronizationContext.Current)
                action();
            else
                Android.App.Application.SynchronizationContext.Post(ignored => ExceptionMaskedAction(action), null);

            return true;
        }
    }
}