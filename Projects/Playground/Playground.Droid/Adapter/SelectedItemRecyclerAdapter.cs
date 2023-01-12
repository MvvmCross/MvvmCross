// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Runtime;
using Android.Widget;
using AndroidX.Core.View;
using AndroidX.RecyclerView.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace Playground.Droid.Adapter
{
    public partial class SelectedItemRecyclerAdapter : MvxRecyclerAdapter
    {
        public event EventHandler<SelectedItemEventArgs> OnItemClick;

        public SelectedItemRecyclerAdapter(IMvxAndroidBindingContext bindingContext)
              : base(bindingContext)
        {
        }

        [Android.Runtime.Preserve(Conditional = true)]
        protected SelectedItemRecyclerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var itemLogo = holder.ItemView.FindViewById<ImageView>(Resource.Id.img_logo);
            AndroidX.Core.View.ViewCompat.SetTransitionName(itemLogo, "anim_img" + position);

            base.OnBindViewHolder(holder, position);
        }

        protected override void OnItemViewClick(object sender, EventArgs e)
        {
            base.OnItemViewClick(sender, e);

            var holder = (MvxRecyclerViewHolder)sender;
            OnItemClick?.Invoke(this, new SelectedItemEventArgs(holder.AdapterPosition, holder.ItemView, holder.DataContext));
        }
    }
}
