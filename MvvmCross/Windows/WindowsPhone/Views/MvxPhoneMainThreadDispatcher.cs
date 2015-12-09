// MvxPhoneMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsPhone.Views
{
    using System;
    using System.Windows.Threading;

    using MvvmCross.Platform.Core;

    public class MvxPhoneMainThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly Dispatcher _uiDispatcher;

        public MvxPhoneMainThreadDispatcher(Dispatcher uiDispatcher)
        {
            this._uiDispatcher = uiDispatcher;
        }

        public bool RequestMainThreadAction(Action action)
        {
            return this.InvokeOrBeginInvoke(action);
        }

        private bool InvokeOrBeginInvoke(Action action)
        {
            if (this._uiDispatcher.CheckAccess())
                ExceptionMaskedAction(action);
            else
                this._uiDispatcher.BeginInvoke(action);

            return true;
        }
    }
}