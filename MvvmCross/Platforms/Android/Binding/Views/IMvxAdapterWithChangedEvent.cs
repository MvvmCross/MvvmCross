// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    public interface IMvxAdapterWithChangedEvent
        : IMvxAdapter
    {
        event EventHandler<NotifyCollectionChangedEventArgs> DataSetChanged;
    }
}
