// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base;

namespace MvvmCross.UnitTest.Mocks.Dispatchers
{
    public class CallbackMockMainThreadDispatcher
        : MvxMainThreadAsyncDispatcher
    {
        private readonly Func<Action, bool> _callback;

        public CallbackMockMainThreadDispatcher(Func<Action, bool> callback)
        {
            _callback = callback;
        }

        public override bool IsOnMainThread => true;

        public override bool RequestMainThreadAction(Action action,
                                                    bool maskExceptions = true)
        {
            return _callback(action);
        }
    }
}
