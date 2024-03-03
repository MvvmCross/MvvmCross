// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.DroidX.RecyclerView.Model;

public class MvxViewHolderBoundEventArgs(
    int itemPosition,
    object? dataContext,
    AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder holder)
    : EventArgs
{
    public int ItemPosition { get; } = itemPosition;

    public object? DataContext { get; } = dataContext;

    public AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder Holder { get; } = holder;
}
