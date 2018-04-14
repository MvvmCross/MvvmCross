// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace Playground.Droid.Adapter
{
    public partial class SelectedItemRecyclerAdapter
    {
        public class SelectedItemViewHolder : MvxRecyclerViewHolder
        {
            private readonly Action<int, View, object> _listener;

            public SelectedItemViewHolder(View itemView, IMvxAndroidBindingContext context, Action<int, View, object> listener)
                : base(itemView, context)
            {
                _listener = listener;
                ItemView.Click += ItemView_Click;
            }

            private void ItemView_Click(object sender, EventArgs e)
                => _listener(AdapterPosition, ItemView, DataContext);

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    ItemView.Click -= ItemView_Click;

                base.Dispose(disposing);
            }
        }
    }
}
