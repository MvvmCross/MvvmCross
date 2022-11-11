// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Core
{
#nullable enable
    public class MvxLifetimeEventArgs : EventArgs
    {
        public MvxLifetimeEventArgs(MvxLifetimeEvent lifetimeEvent)
        {
            LifetimeEvent = lifetimeEvent;
        }

        public MvxLifetimeEvent LifetimeEvent { get; }
    }
#nullable restore
}
