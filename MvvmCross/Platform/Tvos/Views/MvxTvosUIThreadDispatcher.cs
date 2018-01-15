// MvxTvosUIThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;
using MvvmCross.Platform.Core;
using UIKit;

namespace MvvmCross.tvOS.Views
{
    public abstract class MvxTvosUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxTvosUIThreadDispatcher()
        {
            _uiSynchronizationContext = SynchronizationContext.Current;
        }

        public bool RequestMainThreadAction(Action action, bool maskException = true)
        {
            if (_uiSynchronizationContext == SynchronizationContext.Current)
                action();
            else
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() => 
            {
                if (maskException)
                    ExceptionMaskedAction(action);
                else
                    action();
            });
            return true;
        }
    }
}