using System;

namespace Cirrious.MvvmCross.Interfaces.Platform.Lifetime
{
    public class MvxLifetimeEventArgs : EventArgs
    {
        public MvxLifetimeEvent LifetimeEvent { get; private set; }

        public MvxLifetimeEventArgs(MvxLifetimeEvent lifetimeEvent)
        {
            LifetimeEvent = lifetimeEvent;
        }
    }
}