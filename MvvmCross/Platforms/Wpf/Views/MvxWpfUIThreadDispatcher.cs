// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using MvvmCross.Base;

namespace MvvmCross.Platforms.Wpf.Views
{
    public class MvxWpfUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly Dispatcher _dispatcher;

        public MvxWpfUIThreadDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override bool IsOnMainThread => _dispatcher.CheckAccess();

        public override void ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                ExceptionMaskedAction(action, maskExceptions);
            }
            else
            {
                _dispatcher.Invoke(() =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                });
            }
        }

        public override async ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(true);
            }
            else
            {
                var doSomething = _dispatcher.Invoke<ValueTask>(() =>
                {
                    return ExceptionMaskedActionAsync(action, maskExceptions);
                });

                await doSomething;

                //Action aa = () =>
                //{

                //};

                //await _dispatcher.BeginInvoke(DispatcherPriority.Normal, aa);

                //var doSomething = _dispatcher.InvokeAsync<ValueTask>(() =>
                //{
                //    return ExceptionMaskedActionAsync(action, maskExceptions);
                //});

                //doSomething.Wait();
            }
        }
    }
}
