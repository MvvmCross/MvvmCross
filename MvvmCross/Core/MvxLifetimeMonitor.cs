// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Core
{
    public abstract class MvxLifetimeMonitor : IMvxLifetime
    {
        protected void FireLifetimeChange(MvxLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
    }
}