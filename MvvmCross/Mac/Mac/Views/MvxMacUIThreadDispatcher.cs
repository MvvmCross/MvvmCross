// <copyright file="MvxTouchUIThreadDispatcher.cs" company="">
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>


#if __UNIFIED__
using AppKit;
#else
#endif

namespace MvvmCross.Mac.Views
{
    using System;
    using System.Threading;

    using global::MvvmCross.Platform.Core;

    public abstract class MvxMacUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxMacUIThreadDispatcher()
        {
            this._uiSynchronizationContext = SynchronizationContext.Current;
        }

        public bool RequestMainThreadAction(Action action)
        {
            if (this._uiSynchronizationContext == SynchronizationContext.Current)
                action();
            else
                NSApplication.SharedApplication.BeginInvokeOnMainThread(() => ExceptionMaskedAction(action));
            return true;
        }
    }
}