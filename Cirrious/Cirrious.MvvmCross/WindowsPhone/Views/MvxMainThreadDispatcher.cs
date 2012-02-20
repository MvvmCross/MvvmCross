#region Copyright
// <copyright file="MvxMainThreadDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
#region using

using System;
using System.Windows.Threading;
using Cirrious.MvvmCross.Interfaces.Views;

#endregion

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

        protected bool InvokeOrBeginInvoke(Action action)
        {
            if (_uiDispatcher.CheckAccess())
                action();
            else
                _uiDispatcher.BeginInvoke(action);

            return true;
        }
    }
}