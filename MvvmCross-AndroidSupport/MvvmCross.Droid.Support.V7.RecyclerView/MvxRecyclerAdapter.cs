// MvxRecyclerAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping.DataConverters;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemSources;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using MvvmCross.Droid.Support.V7.RecyclerView.Model;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using Object = Java.Lang.Object;

namespace MvvmCross.Droid.Support.V7.RecyclerView
{
    [Register("mvvmcross.droid.support.v7.recyclerview.MvxRecyclerAdapter")]
    public class MvxRecyclerAdapter
        : Android.Support.V7.Widget.RecyclerView.Adapter
        , IMvxRecyclerAdapter
        , IMvxRecyclerAdapterBindableHolder
        , IMvxGroupableRecyclerViewAdapter
        , IMvxHeaderFooterRecyclerViewAdapter
    {
        private readonly MvxRecyclerViewItemsSourceBridgeConfiguration _itemsSourceBridgeConfiguration;

        private ICommand _itemClick,
            _itemLongClick,
            _itemHeaderClickCommand,
            _footerClickCommand,
            _groupHeaderClickCommand;

        private MvxBaseTemplateSelector _itemTemplateSelector;
        private IMvxGroupedDataConverter _groupedDataConverter;

        public MvxRecyclerAdapter() : this(MvxAndroidBindingContextHelpers.Current())
        {
        }

        public MvxRecyclerAdapter(IMvxAndroidBindingContext bindingContext)
        {
            BindingContext = bindingContext;
            _itemsSourceBridgeConfiguration = new MvxRecyclerViewItemsSourceBridgeConfiguration();
            _itemsSourceBridgeConfiguration.ItemsSourceChanged += OnItemsSourceCollectionChanged;
        }

        public MvxRecyclerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected IMvxAndroidBindingContext BindingContext { get; }

        public bool ReloadOnAllItemsSourceSets
        {
            get { return _itemsSourceBridgeConfiguration.ReloadOnAllItemsSourceSets; }
            set { _itemsSourceBridgeConfiguration.ReloadOnAllItemsSourceSets = value; }
        }

        private IMvxRecyclerViewItemsSourceBridge ItemsSourceBridge => _itemsSourceBridgeConfiguration.ItemsSourceBridge;

        public override int ItemCount => ItemsSourceBridge.ItemsCount;

        public ICommand GroupHeaderClickCommand
        {
            get { return _groupHeaderClickCommand; }
            set
            {
                if (ReferenceEquals(_groupHeaderClickCommand, value))
                    return;

                if (_groupHeaderClickCommand != null)
                    MvxTrace.Warning(
                        $"Changing {nameof(GroupHeaderClickCommand)} may cause inconsistencies where some items still call the old command.");

                _groupHeaderClickCommand = value;
            }
        }

        public IMvxGroupedDataConverter GroupedDataConverter
        {
            get { return _itemsSourceBridgeConfiguration.GroupedDataConverter; }
            set { _itemsSourceBridgeConfiguration.GroupedDataConverter = value; }
        }

        public ICommand HeaderClickCommand
        {
            get { return _itemHeaderClickCommand; }
            set
            {
                if (ReferenceEquals(_itemHeaderClickCommand, value))
                    return;

                if (_itemHeaderClickCommand != null)
                    MvxTrace.Warning(
                        $"Changing {nameof(HeaderClickCommand)} may cause inconsistencies where some items still call the old command.");

                _itemHeaderClickCommand = value;
            }
        }

        public ICommand FooterClickCommand
        {
            get { return _footerClickCommand; }
            set
            {
                if (ReferenceEquals(_footerClickCommand, value))
                    return;

                if (_footerClickCommand != null)
                    MvxTrace.Warning(
                        $"Changing {nameof(FooterClickCommand)} may cause inconsistencies where some items still call the old command.");

                _footerClickCommand = value;
            }
        }

        public bool HidesHeaderIfEmpty
        {
            get { return _itemsSourceBridgeConfiguration.HidesHeaderIfEmpty; }
            set { _itemsSourceBridgeConfiguration.HidesHeaderIfEmpty = value; }
        }

        public bool HidesFooterIfEmpty
        {
            get { return _itemsSourceBridgeConfiguration.HidesFooterIfEmpty; }
            set { _itemsSourceBridgeConfiguration.HidesFooterIfEmpty = value; }
        }

        public ICommand ItemClick
        {
            get { return _itemClick; }
            set
            {
                if (ReferenceEquals(_itemClick, value))
                    return;

                if (_itemClick != null)
                    MvxTrace.Warning(
                        $"Changing {nameof(ItemClick)} may cause inconsistencies where some items still call the old command.");

                _itemClick = value;
            }
        }

        public ICommand ItemLongClick
        {
            get { return _itemLongClick; }
            set
            {
                if (ReferenceEquals(_itemLongClick, value))
                    return;

                if (_itemLongClick != null)
                    MvxTrace.Warning(
                        $"Changing {nameof(ItemLongClick)} may cause inconsistencies where some items still call the old command.");

                _itemLongClick = value;
            }
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return ItemsSourceBridge.GetItemsSource(); }
            set { ItemsSourceBridge.SetInternalItemsSource(value ?? Enumerable.Empty<object>()); }
        }

        public virtual MvxBaseTemplateSelector ItemTemplateSelector
        {
            get { return _itemTemplateSelector; }
            set
            {
                if (ReferenceEquals(_itemTemplateSelector, value))
                    return;

                _itemTemplateSelector = value;
                _itemsSourceBridgeConfiguration.IsHeaderEnabled = _itemTemplateSelector.HasHeaderLayoutId;
                _itemsSourceBridgeConfiguration.IsFooterEnabled = _itemTemplateSelector.HasFooterLayoutId;

                // since the template selector has changed then let's force the list to redisplay by firing NotifyDataSetChanged()
                NotifyDataSetChanged();
            }
        }

        public virtual object GetItem(int position) => ItemsSourceBridge.GetItemAt(position);

        public int ItemTemplateId { get; set; }

        public override void OnViewAttachedToWindow(Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnAttachedToWindow();
        }

        public override void OnViewDetachedFromWindow(Object holder)
        {
            base.OnViewDetachedFromWindow(holder);

            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnDetachedFromWindow();
        }

        public override void OnViewRecycled(Object holder)
        {
            base.OnViewRecycled(holder);

            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnViewRecycled();
        }

        public override Android.Support.V7.Widget.RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent,
            int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);

            var vh = new MvxRecyclerViewHolder(InflateViewForHolder(parent, viewType, itemBindingContext), itemBindingContext, viewType)
            {
                Click = ItemClick,
                LongClick = ItemLongClick,
                FooterClickCommand = FooterClickCommand,
                HeaderClickCommand = HeaderClickCommand,
                GroupHeaderClickCommand = GroupHeaderClickCommand
            };

            return vh;
        }

        public override int GetItemViewType(int position)
        {
            var itemAtPosition = GetItem(position);
            return ItemTemplateSelector.GetViewType(itemAtPosition);
        }

        protected virtual View InflateViewForHolder(ViewGroup parent, int viewType,
            IMvxAndroidBindingContext bindingContext)
        {
            var layoutId = ItemTemplateSelector.GetLayoutId(viewType);
            return bindingContext.BindingInflate(layoutId, parent, false);
        }

        public override void OnBindViewHolder(Android.Support.V7.Widget.RecyclerView.ViewHolder holder, int position)
        {
            var dataContext = GetItem(position);
            ((IMvxRecyclerViewHolder)holder).DataContext = dataContext;
            OnMvxViewHolderBound(new MvxViewHolderBoundEventArgs(position, dataContext, holder));
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged(e);
        }

        public virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        for (var i = 0; i < e.NewItems.Count; i++)
                        {
                            var oldItem = e.OldItems[i];
                            var newItem = e.NewItems[i];

                            NotifyItemMoved(ItemsSource.GetPosition(oldItem), ItemsSource.GetPosition(newItem));
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        NotifyItemRangeChanged(e.NewStartingIndex, e.NewItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        NotifyDataSetChanged();
                        break;
                }
            }
            catch (Exception exception)
            {
                Mvx.Warning(
                    "Exception masked during Adapter RealNotifyDataSetChanged {0}. Are you trying to update your collection from a background task? See http://goo.gl/0nW0L6",
                    exception.ToLongString());
            }
        }

        public event Action<MvxViewHolderBoundEventArgs> MvxViewHolderBound;

        protected virtual void OnMvxViewHolderBound(MvxViewHolderBoundEventArgs obj)
        {
            MvxViewHolderBound?.Invoke(obj);
        }
    }
}