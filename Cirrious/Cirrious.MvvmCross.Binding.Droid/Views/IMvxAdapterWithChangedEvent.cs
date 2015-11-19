// IMvxAdapterWithChangedEvent.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Specialized;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public interface IMvxAdapterWithChangedEvent
        : IMvxAdapter
    {
        event EventHandler<NotifyCollectionChangedEventArgs> DataSetChanged;
    }
}