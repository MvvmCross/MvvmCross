// MvxLifetimeEventArgs.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Interfaces.Platform.Lifetime
{
#warning Move to Plugin (or lose!)
    public class MvxLifetimeEventArgs : EventArgs
    {
        public MvxLifetimeEventArgs(MvxLifetimeEvent lifetimeEvent)
        {
            LifetimeEvent = lifetimeEvent;
        }

        public MvxLifetimeEvent LifetimeEvent { get; private set; }
    }
}