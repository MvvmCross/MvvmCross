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

        public MvxWpfUIThreadDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override bool IsOnMainThread => _dispatcher.CheckAccess();

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
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

            // TODO - why return bool at all?
            return true;
        }
    }
}
