// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;
using MvvmCross.Binding.Attributes;
using MvvmCross.DroidX.RecyclerView.AttributeHelpers;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MvvmCross.DroidX.RecyclerView
{
    [Register("mvvmcross.droidx.recyclerview.MvxRecyclerView")]
    public class MvxRecyclerView : AndroidX.RecyclerView.Widget.RecyclerView
    {
        public MvxRecyclerView(Context context, IAttributeSet attrs) :
            this(context, attrs, 0, new MvxRecyclerAdapter())
        {
        }

        public MvxRecyclerView(Context context, IAttributeSet attrs, int defStyle) 
            : this(context, attrs, defStyle, new MvxRecyclerAdapter())
        {
        }

        [Android.Runtime.Preserve(Conditional = true)]
        protected MvxRecyclerView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public MvxRecyclerView(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter) 
            : base(context, attrs, defStyle)
        {
            // Note: Any calling derived class passing a null adapter is responsible for setting
            // it's own ItemTemplateSelector
            if (adapter == null)
                return;

            var currentLayoutManager = GetLayoutManager();
            if (currentLayoutManager == null)
                SetLayoutManager(new MvxGuardedLinearLayoutManager(context));

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            var itemTemplateSelector = MvxRecyclerViewAttributeExtensions.BuildItemTemplateSelector(context, attrs, itemTemplateId);

            adapter.ItemTemplateSelector = itemTemplateSelector;
            Adapter = adapter;

            if (itemTemplateId == 0)
                itemTemplateId = global::Android.Resource.Layout.SimpleListItem1;

            if (itemTemplateSelector.GetType() == typeof(MvxDefaultTemplateSelector))
                ItemTemplateId = itemTemplateId;
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            DetachedFromWindow();
        }

        protected virtual void DetachedFromWindow()
        {
            // Remove all the views that are currently in play.
            // This clears out all of the ViewHolder DataContexts by detaching the ViewHolder.
            // Eventually the GC will come along and clear out the binding contexts.
            // Issue #1405
             //Note: this has a side effect of breaking fragment transitions, as the recyclerview is cleared before the transition starts, which empties the view and displays a "black" screen while transitioning.
            GetLayoutManager()?.RemoveAllViews();
        }

        [MvxSetToNullAfterBinding]
        public new IMvxRecyclerAdapter Adapter
        {
            get => GetAdapter() as IMvxRecyclerAdapter;
            set
            {
                var existing = Adapter;
                if (existing == value)
                    return;

                // Support lib doesn't seem to have anything similar to IListAdapter yet
                // hence cast to Adapter.
                if (value != null && existing != null)
                {
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemTemplateSelector = existing.ItemTemplateSelector;
                    value.ItemClick = existing.ItemClick;
                    value.ItemLongClick = existing.ItemLongClick;

                    SwapAdapter((Adapter)value, false);
                }
                else
                {
                    SetAdapter((Adapter)value);
                }

                if (existing != null)
                    existing.ItemsSource = null;
            }
        }

        [MvxSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get => Adapter.ItemsSource;
            set
            {
                var adapter = Adapter;
                if (adapter != null)
                    adapter.ItemsSource = value;
            }
        }

        public int ItemTemplateId
        {
            get
            {
                if (!(ItemTemplateSelector is MvxDefaultTemplateSelector singleItemDefaultTemplateSelector))
                    throw new InvalidOperationException(
                        $"If you don't want to use single item-template RecyclerView Adapter you can't change it's" +
                        $"{nameof(IMvxTemplateSelector)} to anything other than {nameof(MvxDefaultTemplateSelector)}");

                return singleItemDefaultTemplateSelector.ItemTemplateId;
            }
            set
            {
                if (!(ItemTemplateSelector is MvxDefaultTemplateSelector singleItemDefaultTemplateSelector))
                    throw new InvalidOperationException(
                        $"If you don't want to use single item-template RecyclerView Adapter you can't change it's" +
                        $"{nameof(IMvxTemplateSelector)} to anything other than {nameof(MvxDefaultTemplateSelector)}");

                singleItemDefaultTemplateSelector.ItemTemplateId = value;
                Adapter.ItemTemplateSelector = singleItemDefaultTemplateSelector;
            }
        }

        public IMvxTemplateSelector ItemTemplateSelector
        {
            get => Adapter.ItemTemplateSelector;
            set => Adapter.ItemTemplateSelector = value;
        }

        [MvxSetToNullAfterBinding]
        public ICommand ItemClick
        {
            get => Adapter.ItemClick;
            set => Adapter.ItemClick = value;
        }

        [MvxSetToNullAfterBinding]
        public ICommand ItemLongClick
        {
            get => Adapter.ItemLongClick;
            set => Adapter.ItemLongClick = value;
        }
    }
}
