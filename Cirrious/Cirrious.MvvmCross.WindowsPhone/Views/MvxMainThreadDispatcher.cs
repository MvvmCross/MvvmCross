﻿// MvxMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Threading;
using Cirrious.CrossCore.Interfaces.Core;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public class MvxMainThreadDispatcher : IMvxMainThreadDispatcher
    {
        private readonly Dispatcher _uiDispatcher;

        public MvxMainThreadDispatcher(Dispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }

        public bool RequestMainThreadAction(Action action)
        {
            return InvokeOrBeginInvoke(action);
        }

        private bool InvokeOrBeginInvoke(Action action)
        {
            if (_uiDispatcher.CheckAccess())
                action();
            else
                _uiDispatcher.BeginInvoke(action);

            return true;
        }
    }
}