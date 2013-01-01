// MvxBaseLifetimeMonitor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;

namespace Cirrious.MvvmCross.Platform.Lifetime
{
    public abstract class MvxBaseLifetimeMonitor : IMvxLifetime
    {
        protected void FireLifetimeChange(MvxLifetimeEvent which)
        {
            var handler = LifetimeChanged;
            if (handler != null)
                handler(this, new MvxLifetimeEventArgs(which));
        }

        #region Implementation of IMvxLifetime

        public event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;

        #endregion
    }
}