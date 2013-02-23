using System;
using Cirrious.CrossCore.Interfaces.Core;

namespace Cirrious.MvvmCross.Droid.Fragging
{
    public interface IMvxEventSourceFragment : IMvxDisposeSource
    {
        event EventHandler<MvxValueEventArgs<MvxCreateViewParameters>>  OnCreateViewCalled;
        event EventHandler OnDestroyViewCalled; 
    }
}