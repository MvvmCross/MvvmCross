using System;

namespace Cirrious.MvvmCross.Interfaces.Services.Lifetime
{
    public interface IMvxLifetime
    {
        event EventHandler<MvxLifetimeEventArgs> LifetimeChanged;
    }
}