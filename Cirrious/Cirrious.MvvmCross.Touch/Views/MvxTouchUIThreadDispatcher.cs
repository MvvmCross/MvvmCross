// MvxTouchUIThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using System;
using System.Threading;
using UIKit;

namespace Cirrious.MvvmCross.Touch.Views
{
    public abstract class MvxTouchUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxTouchUIThreadDispatcher()
        {
            _uiSynchronizationContext = SynchronizationContext.Current;
        }

        public bool RequestMainThreadAction(Action action)
        {
            if (_uiSynchronizationContext == SynchronizationContext.Current)
                action();
            else
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() => ExceptionMaskedAction(action));
            return true;
        }
    }
}