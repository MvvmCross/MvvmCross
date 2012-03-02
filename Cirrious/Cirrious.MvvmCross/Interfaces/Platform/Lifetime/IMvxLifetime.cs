using System;

namespace Cirrious.MvvmCross.Interfaces.Platform.Lifetime
{
    public interface IMvxLifetime
    {
        event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
    }
}