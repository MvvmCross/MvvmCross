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
#nullable enable
    [Register("mvvmcross.droidx.recyclerview.MvxRecyclerView")]
    public class MvxRecyclerView : AndroidX.RecyclerView.Widget.RecyclerView
    {
        public MvxRecyclerView(Context context, IAttributeSet attrs)
            : this(context, attrs, 0, new MvxRecyclerAdapter())
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

        /// <summary>
        /// Create an instance of MvxRecyclerView.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStyle"></param>
        /// <param name="adapter"><para><see cref="IMvxRecyclerAdapter"/> to use.</para>
        /// <para>If this is set to <code>null</code>, then it is up to you setting a <see cref="ItemTemplateSelector"/>.</para></param>
        public MvxRecyclerView(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter)
            : base(context, attrs, defStyle)
        {
            // Note: Any calling derived class passing a null adapter is responsible for setting
            // it's own ItemTemplateSelector
            if (adapter == null)
                return;

            var currentLayoutManager = GetLayoutManager();
            if (currentLayoutManager == null)
#pragma warning disable CA2000 // Dispose objects before losing scope
                SetLayoutManager(new MvxGuardedLinearLayoutManager(context));
#pragma warning restore CA2000 // Dispose objects before losing scope

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
#pragma warning disable CA1721 // Property names should not match get methods
        public new IMvxRecyclerAdapter? Adapter
#pragma warning restore CA1721 // Property names should not match get methods
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
                    SetAdapter(value as Adapter);
                }

                if (existing != null)
                {
                    existing.ItemsSource = null;
                }
            }
        }

        /// <summary>
        /// <para>Get or set the ItemSource to use for the RecyclerView Adapter.</para>
        /// <para>
        /// It is recommended to use a type inheriting from <see cref="IList"/>, such as
        /// <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>,
        /// <see cref="MvvmCross.ViewModels.MvxObservableCollection{T}"/> or
        /// <see cref="System.Collections.Generic.List{T}"/>.
        /// </para>
        /// </summary>
        [MvxSetToNullAfterBinding]
        public IEnumerable? ItemsSource
        {
            get => Adapter?.ItemsSource;
            set
            {
                var adapter = Adapter;
                if (adapter != null)
                    adapter.ItemsSource = value;
            }
        }

        /// <summary>
        /// <para>Get or set the Item Template Id for cases where you only have one type of view in the RecyclerView.</para>
        /// </summary>
        public int ItemTemplateId
        {
            get
            {
                if (!(ItemTemplateSelector is MvxDefaultTemplateSelector singleItemDefaultTemplateSelector))
                {
                    throw new InvalidOperationException(
                        "If you don't want to use single item-template RecyclerView Adapter you can't change it's" +
                        $"{nameof(IMvxTemplateSelector)} to anything other than {nameof(MvxDefaultTemplateSelector)}");
                }

                return singleItemDefaultTemplateSelector.ItemTemplateId;
            }
            set
            {
                if (!(ItemTemplateSelector is MvxDefaultTemplateSelector singleItemDefaultTemplateSelector))
                {
                    throw new InvalidOperationException(
                        "If you don't want to use single item-template RecyclerView Adapter you can't change it's" +
                        $"{nameof(IMvxTemplateSelector)} to anything other than {nameof(MvxDefaultTemplateSelector)}");
                }

                singleItemDefaultTemplateSelector.ItemTemplateId = value;

                if (Adapter != null)
                {
                    Adapter.ItemTemplateSelector = singleItemDefaultTemplateSelector;
                }
            }
        }

        /// <summary>
        /// <para>Get or set the ItemTemplateSelector.</para>
        /// </summary>
        public IMvxTemplateSelector? ItemTemplateSelector
        {
            get => Adapter?.ItemTemplateSelector;
            set
            {
                if (Adapter != null)
                {
                    Adapter.ItemTemplateSelector = value;
                }
            }
        }

        /// <summary>
        /// Get or set the <see cref="ICommand"/> to trigger when an item was clicked.
        /// </summary>
        [MvxSetToNullAfterBinding]
        public ICommand? ItemClick
        {
            get => Adapter?.ItemClick;
            set
            {
                if (Adapter != null)
                {
                    Adapter.ItemClick = value;
                }
            }
        }

        /// <summary>
        /// Get or set the <see cref="ICommand"/> to trigger when an item was long clicked.
        /// </summary>
        [MvxSetToNullAfterBinding]
        public ICommand? ItemLongClick
        {
            get => Adapter?.ItemLongClick;
            set
            {
                if (Adapter != null)
                {
                    Adapter.ItemLongClick = value;
                }
            }
        }
    }
#nullable restore
}
