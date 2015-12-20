// IMvxAdapterWithChangedEvent.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Views
{
    using System;
    using System.Collections.Specialized;

    public interface IMvxAdapterWithChangedEvent
        : IMvxAdapter
    {
        event EventHandler<NotifyCollectionChangedEventArgs> DataSetChanged;
    }
}