// MvxAndroidMainThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;
using Android.App;
using MvvmCross.Platform.Core;

namespace MvvmCross.Droid.Views
{
    public class MvxAndroidMainThreadDispatcher : MvxMainThreadDispatcher
    {
        public bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (Application.SynchronizationContext == SynchronizationContext.Current)
                action();
            else
            {
                Application.SynchronizationContext.Post(ignored => 
                {
                    if (maskExceptions)
                        ExceptionMaskedAction(action);
                    else
                        action();
                }, null);
            }

            return true;
        }
    }
}
