#region Copyright

// <copyright file="MvxLifetimeEventArgs.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;

namespace Cirrious.MvvmCross.Interfaces.Platform.Lifetime
{
    public class MvxLifetimeEventArgs : EventArgs
    {
        public MvxLifetimeEventArgs(MvxLifetimeEvent lifetimeEvent)
        {
            LifetimeEvent = lifetimeEvent;
        }

        public MvxLifetimeEvent LifetimeEvent { get; private set; }
    }
}