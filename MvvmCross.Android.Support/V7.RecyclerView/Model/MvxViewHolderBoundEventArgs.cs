// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Droid.Support.V7.RecyclerView.Model
{
    public class MvxViewHolderBoundEventArgs
    {
        public MvxViewHolderBoundEventArgs(int itemPosition, object dataContext, Android.Support.V7.Widget.RecyclerView.ViewHolder holder)
        {
            ItemPosition = itemPosition;
            DataContext = dataContext;
            Holder = holder;
        }

        public int ItemPosition { get; }

        public object DataContext { get; }

        public Android.Support.V7.Widget.RecyclerView.ViewHolder Holder { get; }
    }
}