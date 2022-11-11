// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Core
{
#nullable enable
    public abstract class MvxLifetimeMonitor : IMvxLifetime
    {
#pragma warning disable CA1030 // Use events where appropriate
        protected void FireLifetimeChange(MvxLifetimeEvent which)
#pragma warning restore CA1030 // Use events where appropriate
        {
            LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        public event EventHandler<MvxLifetimeEventArgs>? LifetimeChanged;
    }
#nullable restore
}
