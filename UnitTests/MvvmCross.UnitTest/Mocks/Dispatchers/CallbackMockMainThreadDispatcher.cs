// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Base;

namespace MvvmCross.UnitTest.Mocks.Dispatchers
{
    public class CallbackMockMainThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly Func<Action, bool> _callback;

        public CallbackMockMainThreadDispatcher(Func<Action, bool> callback)
        {
            _callback = callback;
        }

        public override bool IsOnMainThread => true;

        public override ValueTask ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            _callback(() =>
            {
                ExceptionMaskedAction(action, maskExceptions);
            });

            return new ValueTask();
        }

        public override ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            _callback(async () =>
            {
                await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(false);
            });

            return new ValueTask();
        }
    }
}
