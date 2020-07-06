// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MvvmCross.Base;

namespace MvvmCross.UnitTest.Mocks.Dispatchers
{
    public class CountingMockMainThreadDispatcher
        : MvxMainThreadDispatcher
    {
        public int Count { get; set; }

        public override bool IsOnMainThread => true;

        public override void ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            ExceptionMaskedAction(action, maskExceptions);
            Count++;
        }

        public override async ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(false);
            Count++;
        }
    }
}
