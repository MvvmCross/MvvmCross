// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;
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

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
            View view = InflateViewForHolder(parent, viewType, itemBindingContext);

            return new SelectedItemViewHolder(view, itemBindingContext, OnClick)
            {
                Click = ItemClick,
                LongClick = ItemLongClick
            };
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ImageView itemLogo = holder.ItemView.FindViewById<ImageView>(Resource.Id.img_logo);
            ViewCompat.SetTransitionName(itemLogo, "anim_img" + position);

            base.OnBindViewHolder(holder, position);
        }

        private void OnClick(int position, View view, object dataContext)
            => OnItemClick?.Invoke(this, new SelectedItemEventArgs(position, view, dataContext));
    }
}
