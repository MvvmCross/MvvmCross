// MvxIosUIThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;
using MvvmCross.Platform.Core;
using UIKit;

namespace MvvmCross.iOS.Views
{
    public abstract class MvxIosUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxIosUIThreadDispatcher()
        {
            _uiSynchronizationContext = SynchronizationContext.Current;
        }

        public bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (_uiSynchronizationContext == SynchronizationContext.Current)
                action();
            else
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() => 
                {
                    if (maskExceptions)
                        ExceptionMaskedAction(action);
                    else
                        action();
                });
            return true;
        }
    }
}