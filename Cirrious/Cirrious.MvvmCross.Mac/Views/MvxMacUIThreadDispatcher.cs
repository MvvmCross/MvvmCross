// <copyright file="MvxTouchUIThreadDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

using Cirrious.CrossCore.Core;
using System;
using System.Threading;

#if __UNIFIED__
using AppKit;
#else
#endif

namespace Cirrious.MvvmCross.Mac.Views
{
    public abstract class MvxMacUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxMacUIThreadDispatcher()
        {
            _uiSynchronizationContext = SynchronizationContext.Current;
        }

        public bool RequestMainThreadAction(Action action)
        {
            if (_uiSynchronizationContext == SynchronizationContext.Current)
                action();
            else
                NSApplication.SharedApplication.BeginInvokeOnMainThread(() => ExceptionMaskedAction(action));
            return true;
        }
    }
}