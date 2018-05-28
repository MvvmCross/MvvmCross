// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Threading;
using MvvmCross.Base;

namespace MvvmCross.Platforms.Wpf.Views
{
    public class MvxWpfUIThreadDispatcher
        : MvxMainThreadAsyncDispatcher
    {
        private readonly Dispatcher _dispatcher;

        public MvxWpfUIThreadDispatcher(Dispatcher dispatcher, int manangedThreadId) : base(manangedThreadId)
        {
            _dispatcher = dispatcher;
        }

        public override void RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (_dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                _dispatcher.Invoke(() =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                });
            }
        }
    }
}
