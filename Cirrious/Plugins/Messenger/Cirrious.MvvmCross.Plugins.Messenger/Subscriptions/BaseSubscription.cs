#region Copyright

// <copyright file="BaseSubscription.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;

namespace Cirrious.MvvmCross.Plugins.Messenger.Subscriptions
{
    public abstract class BaseSubscription
    {
        public Guid Id { get; private set; }
        public abstract bool IsAlive { get; }
        public abstract bool Invoke(object message);

        protected BaseSubscription()
        {
            Id = Guid.NewGuid();
        }
    }
}