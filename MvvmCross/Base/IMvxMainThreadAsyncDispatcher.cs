// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace MvvmCross.Base
{
    // Note: The long term goal should be to deprecate IMvxMainThreadDispatcher
    // As such, even though the implementation of this interface also implements
    // IMvxMainThreadDispatcher, this interface should not inherit from IMvxMainThreadDispatcher
    public interface IMvxMainThreadAsyncDispatcher
    {
        Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true);
        Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true);
        bool IsOnMainThread { get; }
    }
}
