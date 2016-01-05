// MvxLifetimeMonitor.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.Platform
{
    using System;

    public abstract class MvxLifetimeMonitor : IMvxLifetime
    {
        protected void FireLifetimeChange(MvxLifetimeEvent which)
        {
            var handler = this.LifetimeChanged;
            handler?.Invoke(this, new MvxLifetimeEventArgs(which));
        }

        #region Implementation of IMvxLifetime

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        #endregion Implementation of IMvxLifetime
    }
}