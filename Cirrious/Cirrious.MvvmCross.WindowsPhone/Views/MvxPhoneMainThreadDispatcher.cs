// MvxPhoneMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Core;
using System;
using System.Windows.Threading;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public class MvxPhoneMainThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly Dispatcher _uiDispatcher;

        public MvxPhoneMainThreadDispatcher(Dispatcher uiDispatcher)
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
                ExceptionMaskedAction(action);
            else
                _uiDispatcher.BeginInvoke(action);

            return true;
        }
    }
}