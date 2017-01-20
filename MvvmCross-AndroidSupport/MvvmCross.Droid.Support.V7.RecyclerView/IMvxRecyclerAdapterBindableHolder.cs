using System;
using MvvmCross.Droid.Support.V7.RecyclerView.Model;

namespace MvvmCross.Droid.Support.V7.RecyclerView
{
    public interface IMvxRecyclerAdapterBindableHolder
    {
        event Action<MvxViewHolderBindedEventArgs> MvxViewHolderBinded;
    }
}