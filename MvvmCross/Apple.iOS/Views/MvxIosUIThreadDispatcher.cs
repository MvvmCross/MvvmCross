// MvxIosUIThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views
{
    using System;
    using System.Threading;

    using MvvmCross.Platform.Core;

    using UIKit;

    public abstract class MvxIosUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxIosUIThreadDispatcher()
        {
            this._uiSynchronizationContext = SynchronizationContext.Current;
        }

        public bool RequestMainThreadAction(Action action)
        {
            if (this._uiSynchronizationContext == SynchronizationContext.Current)
                action();
            else
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() => ExceptionMaskedAction(action));
            return true;
        }
    }
}