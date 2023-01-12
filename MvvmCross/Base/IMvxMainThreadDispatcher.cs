// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Base
{
#nullable enable
    public interface IMvxMainThreadDispatcher
    {
        [Obsolete("Use IMvxMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync instead")]
        bool RequestMainThreadAction(Action action, bool maskExceptions = true);
        bool IsOnMainThread { get; }
    }
#nullable restore
}
