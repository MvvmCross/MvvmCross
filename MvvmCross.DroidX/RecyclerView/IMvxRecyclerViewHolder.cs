// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding.BindingContext;

namespace MvvmCross.DroidX.RecyclerView
{
    public interface IMvxRecyclerViewHolder : IMvxBindingContextOwner
    {
        int Id { get; set; }
        object DataContext { get; set; }

        void OnAttachedToWindow();
        void OnDetachedFromWindow();
        void OnViewRecycled();

        event EventHandler<EventArgs> Click;
        event EventHandler<EventArgs> LongClick;
    }
}
