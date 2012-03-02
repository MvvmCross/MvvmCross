#region Copyright
// <copyright file="MvxBaseLifetimeMonitor.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

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