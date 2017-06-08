// MvxAndroidMainThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;

namespace MvvmCross.Droid.Views
{
    using System;
    using System.Threading;

    using MvvmCross.Platform.Core;

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
    }
}