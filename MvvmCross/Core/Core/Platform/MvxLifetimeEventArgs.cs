// MvxLifetimeEventArgs.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.Platform
{
    using System;

    public class MvxLifetimeEventArgs : EventArgs
    {
        public MvxLifetimeEventArgs(MvxLifetimeEvent lifetimeEvent)
        {
            this.LifetimeEvent = lifetimeEvent;
        }

        public MvxLifetimeEvent LifetimeEvent { get; private set; }
    }
}