// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.AndroidXSupport.RecyclerView.Model;

namespace MvvmCross.AndroidXSupport.RecyclerView
{
    public interface IMvxRecyclerAdapterBindableHolder
    {
        event Action<MvxViewHolderBoundEventArgs> MvxViewHolderBound;
    }
}
