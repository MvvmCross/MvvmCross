// MvxRecyclerView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.RecyclerView.AttributeHelpers;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping.DataConverters;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace MvvmCross.Droid.Support.V7.RecyclerView
{
    [Register("mvvmcross.droid.support.v7.recyclerview.MvxRecyclerView")]
    public class MvxRecyclerView : Android.Support.V7.Widget.RecyclerView
    {
        public MvxRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
        public MvxRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, 0, new MvxRecyclerAdapter()) { }
        public MvxRecyclerView(Context context, IAttributeSet attrs, int defStyle) : this(context, attrs, defStyle, new MvxRecyclerAdapter()) { }
        public MvxRecyclerView(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter) : base(context, attrs, defStyle)
        {
            // Note: Any calling derived class passing a null adapter is responsible for setting
            // it's own ItemTemplateSelector
            if (adapter == null)
                return;

            var currentLayoutManager = base.GetLayoutManager();

            // Love you Android
            // https://code.google.com/p/android/issues/detail?id=77846#c10
            // Don't believe those bastards, it's not fixed - workaround hack hack hack
            if (currentLayoutManager == null)
                SetLayoutManager(new MvxGuardedLinearLayoutManager(context));

            if (currentLayoutManager is LinearLayoutManager)
            {
                var currentLinearLayoutManager = currentLayoutManager as LinearLayoutManager;
                SetLayoutManager(new MvxGuardedLinearLayoutManager(context) { Orientation = currentLinearLayoutManager.Orientation });
            }

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            var itemTemplateSelector = MvxRecyclerViewAttributeExtensions.BuildItemTemplateSelector(context, attrs);

            adapter.ItemTemplateSelector = itemTemplateSelector;
            Adapter = adapter;

            if (itemTemplateSelector.GetType() == typeof(MvxDefaultTemplateSelector))
                ItemTemplateId = itemTemplateId;

            HidesHeaderIfEmpty = MvxRecyclerViewAttributeExtensions.IsHidesHeaderIfEmptyEnabled(context, attrs);
            HidesFooterIfEmpty = MvxRecyclerViewAttributeExtensions.IsHidesFooterIfEmptyEnabled(context, attrs);

            if (MvxRecyclerViewAttributeExtensions.IsGroupingSupported(context, attrs))
                GroupedDataConverter = MvxRecyclerViewAttributeExtensions.BuildMvxGroupedDataConverter(context, attrs);
        }

        public sealed override void SetLayoutManager(LayoutManager layout)
        {
            base.SetLayoutManager(layout);
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();

            // Remove all the views that are currently in play.
            // This clears out all of the ViewHolder DataContexts by detaching the ViewHolder.
            // Eventually the GC will come along and clear out the binding contexts.
            // Issue #1405
            GetLayoutManager()?.RemoveAllViews();
        }

        public new IMvxRecyclerAdapter Adapter
        {
            get { return base.GetAdapter() as IMvxRecyclerAdapter; }
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

                    var existingGroupableAdapter = existing as IMvxGroupableRecyclerViewAdapter;
                    var newGroupableAdapter = value as IMvxGroupableRecyclerViewAdapter;

                    if (existingGroupableAdapter != null && newGroupableAdapter != null)
                    {
                        newGroupableAdapter.GroupHeaderClickCommand = existingGroupableAdapter.GroupHeaderClickCommand;
                        newGroupableAdapter.GroupedDataConverter = existingGroupableAdapter.GroupedDataConverter;
                    }

                    var existingHeaderFooterAdapter = existing as IMvxHeaderFooterRecyclerViewAdapter;
                    var newHeaderFooterAdapter = value as IMvxHeaderFooterRecyclerViewAdapter;

                    if (existingGroupableAdapter != null && newHeaderFooterAdapter != null)
                    {
                        newHeaderFooterAdapter.FooterClickCommand = existingHeaderFooterAdapter.FooterClickCommand;
                        newHeaderFooterAdapter.HeaderClickCommand = existingHeaderFooterAdapter.HeaderClickCommand;
                        newHeaderFooterAdapter.HidesFooterIfEmpty = existingHeaderFooterAdapter.HidesFooterIfEmpty;
                        newHeaderFooterAdapter.HidesHeaderIfEmpty = existingHeaderFooterAdapter.HidesHeaderIfEmpty;
                    }

                    SwapAdapter((Adapter)value, false);
                }
                else
                {
                    SetAdapter((Adapter)value);
                }

                if (existing != null)
                {
                    existing.ItemsSource = null;
                }
            }
        }

        [MvxSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get { return Adapter.ItemsSource; }
            set
            {
                Adapter.ItemsSource = value;
            }
        }

        public int ItemTemplateId
        {
            get
            {
                var singleItemDefaultTemplateSelector = ItemTemplateSelector as MvxDefaultTemplateSelector;

                if (singleItemDefaultTemplateSelector == null)
                    throw new InvalidOperationException(
                        $"If you wan't to use single item-template RecyclerView Adapter you can't change it's" +
                        $"{nameof(MvxBaseTemplateSelector)} to anything other than {nameof(MvxDefaultTemplateSelector)}");

                return singleItemDefaultTemplateSelector.ItemTemplateId;
            }
            set
            {
                var singleItemDefaultTemplateSelector = ItemTemplateSelector as MvxDefaultTemplateSelector;

                if (singleItemDefaultTemplateSelector == null)
                    throw new InvalidOperationException(
                        $"If you wan't to use single item-template RecyclerView Adapter you can't change it's" +
                        $"{nameof(MvxBaseTemplateSelector)} to anything other than {nameof(MvxDefaultTemplateSelector)}");

                singleItemDefaultTemplateSelector.ItemTemplateId = value;
                Adapter.ItemTemplateSelector = singleItemDefaultTemplateSelector;
            }
        }

        public int HeaderLayoutId
        {
            get { return ItemTemplateSelector.HeaderLayoutId; }
            set { ItemTemplateSelector.HeaderLayoutId = value; }
        }

        public int FooterLayoutId
        {
            get { return ItemTemplateSelector.FooterLayoutId; }
            set { ItemTemplateSelector.FooterLayoutId = value; }
        }

        public int GroupSectionLayoutId
        {
            get { return ItemTemplateSelector.GroupSectionLayoutId; }
            set { ItemTemplateSelector.GroupSectionLayoutId = value; }
        }

        public bool HidesHeaderIfEmpty
        {
            get { return GetCastedAdapter<IMvxHeaderFooterRecyclerViewAdapter>()?.HidesHeaderIfEmpty ?? false; }
            set
            {
                var headerFooterAdapter = GetCastedAdapter<IMvxHeaderFooterRecyclerViewAdapter>();

                if (headerFooterAdapter != null)
                    headerFooterAdapter.HidesHeaderIfEmpty = value;
            }
        }

        public bool HidesFooterIfEmpty
        {
            get { return GetCastedAdapter<IMvxHeaderFooterRecyclerViewAdapter>()?.HidesFooterIfEmpty ?? false; }
            set
            {
                var headerFooterAdapter = GetCastedAdapter<IMvxHeaderFooterRecyclerViewAdapter>();

                if (headerFooterAdapter != null)
                    headerFooterAdapter.HidesFooterIfEmpty = value;
            }
        }

        public MvxBaseTemplateSelector ItemTemplateSelector
        {
            get { return Adapter.ItemTemplateSelector; }
            set
            {
                var oldTemplateSelector = Adapter.ItemTemplateSelector;
                if (oldTemplateSelector != null && value != null)
                {
                    value.HeaderLayoutId = oldTemplateSelector.HeaderLayoutId;
                    value.FooterLayoutId = oldTemplateSelector.FooterLayoutId;
                    value.GroupSectionLayoutId = oldTemplateSelector.GroupSectionLayoutId;
                }

                Adapter.ItemTemplateSelector = value;
            }
        }

        public ICommand ItemClick
        {
            get { return this.Adapter.ItemClick; }
            set { this.Adapter.ItemClick = value; }
        }

        public ICommand HeaderClickCommand
        {
            get { return GetCastedAdapter<IMvxHeaderFooterRecyclerViewAdapter>()?.HeaderClickCommand; }
            set
            {
                var headerFooterAdapter = GetCastedAdapter<IMvxHeaderFooterRecyclerViewAdapter>();

                if (headerFooterAdapter != null)
                    headerFooterAdapter.HeaderClickCommand = value;
            }
        }

        public ICommand FooterClickCommand
        {
            get { return GetCastedAdapter<IMvxHeaderFooterRecyclerViewAdapter>()?.FooterClickCommand; }
            set
            {
                var headerFooterAdapter = GetCastedAdapter<IMvxHeaderFooterRecyclerViewAdapter>();

                if (headerFooterAdapter != null)
                    headerFooterAdapter.FooterClickCommand = value;
            }
        }

        public ICommand GroupHeaderClickCommand
        {
            get { return GetCastedAdapter<IMvxGroupableRecyclerViewAdapter>()?.GroupHeaderClickCommand; }
            set
            {
                var groupableAdapter = GetCastedAdapter<IMvxGroupableRecyclerViewAdapter>();

                if (groupableAdapter != null)
                    groupableAdapter.GroupHeaderClickCommand = value;
            }
        }

        public IMvxGroupedDataConverter GroupedDataConverter
        {
            get { return GetCastedAdapter<IMvxGroupableRecyclerViewAdapter>()?.GroupedDataConverter; }
            set
            {
                var groupableAdapter = GetCastedAdapter<IMvxGroupableRecyclerViewAdapter>();

                if (groupableAdapter != null)
                    groupableAdapter.GroupedDataConverter = value;
            }
        }

        public ICommand ItemLongClick
        {
            get { return this.Adapter.ItemLongClick; }
            set { this.Adapter.ItemLongClick = value; }
        }

        private T GetCastedAdapter<T>() where T : class, IMvxRecyclerAdapter
        {
            return Adapter as T;
        }
    }
}
