// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Input;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Extensions;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using MvvmCross.Droid.Support.V7.RecyclerView.Model;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.WeakSubscription;
using Object = Java.Lang.Object;

namespace MvvmCross.Droid.Support.V7.RecyclerView
{
    [Register("mvvmcross.droid.support.v7.recyclerview.MvxRecyclerAdapter")]
    public class MvxRecyclerAdapter
        : Android.Support.V7.Widget.RecyclerView.Adapter, IMvxRecyclerAdapter, IMvxRecyclerAdapterBindableHolder
    {
        private ICommand _itemClick, _itemLongClick;
        private IEnumerable _itemsSource;
        private IDisposable _subscription;
        private IMvxTemplateSelector _itemTemplateSelector;

        protected IMvxAndroidBindingContext BindingContext { get; }

        public MvxRecyclerAdapter(IMvxAndroidBindingContext bindingContext = null)
        {
            BindingContext = bindingContext ?? MvxAndroidBindingContextHelpers.Current();
        }

        protected MvxRecyclerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public bool ReloadOnAllItemsSourceSets { get; set; }

        [MvxSetToNullAfterBinding]
        public ICommand ItemClick
        {
            get => _itemClick;
            set
            {
                if (ReferenceEquals(_itemClick, value))
                    return;

                if (_itemClick != null && value != null)
                    MvxAndroidLog.Instance.Warn("Changing ItemClick may cause inconsistencies where some items still call the old command.");

                _itemClick = value;
            }
        }

        [MvxSetToNullAfterBinding]
        public ICommand ItemLongClick
        {
            get => _itemLongClick;
            set
            {
                if (ReferenceEquals(_itemLongClick, value))
                    return;

                if (_itemLongClick != null && value != null)
                    MvxAndroidLog.Instance.Warn("Changing ItemLongClick may cause inconsistencies where some items still call the old command.");

                _itemLongClick = value;
            }
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get => _itemsSource;
            set => SetItemsSource(value);
        }

        [MvxSetToNullAfterBinding]
        public virtual IMvxTemplateSelector ItemTemplateSelector
            {
            get => _itemTemplateSelector;
            set
            {
                if (ReferenceEquals(_itemTemplateSelector, value))
                    return;

                _itemTemplateSelector = value;

                // since the template selector has changed then let's force the list to redisplay by firing NotifyDataSetChanged()
                if (_itemsSource != null)
                    NotifyDataSetChanged();
            }
        }

        public override void OnViewAttachedToWindow(Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnAttachedToWindow();
        }

        public override void OnViewDetachedFromWindow(Object holder)
        {
            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnDetachedFromWindow();
            base.OnViewDetachedFromWindow(holder);
        }

        public override void OnViewRecycled(Object holder)
        {
            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnViewRecycled();
            base.OnViewRecycled(holder);
        }

        public override Android.Support.V7.Widget.RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);

            var vh = new MvxRecyclerViewHolder(InflateViewForHolder(parent, viewType, itemBindingContext), itemBindingContext)
            {
                Click = ItemClick,
                LongClick = ItemLongClick,
                Id = viewType
            };

            return vh;
        }

        public override int GetItemViewType(int position)
        {
            var itemAtPosition = GetItem(position);
            var viewTypeIndex = ItemTemplateSelector.GetItemViewType(itemAtPosition);
            var viewType = ItemTemplateSelector.GetItemLayoutId(viewTypeIndex);
            return viewType;
        }

        protected virtual View InflateViewForHolder(ViewGroup parent, int viewType, IMvxAndroidBindingContext bindingContext)
        {
            return bindingContext.BindingInflate(viewType, parent, false);
        }

        public override void OnBindViewHolder(Android.Support.V7.Widget.RecyclerView.ViewHolder holder, int position)
        {
            var dataContext = GetItem(position);
            if (((IMvxRecyclerViewHolder) holder).Id == global::Android.Resource.Layout.SimpleListItem1)
                ((TextView) holder.ItemView).Text = dataContext?.ToString();
            ((IMvxRecyclerViewHolder)holder).DataContext = dataContext;
            OnMvxViewHolderBound(new MvxViewHolderBoundEventArgs(position, dataContext, holder));
        }

        public override int ItemCount => _itemsSource.Count();

        public virtual object GetItem(int viewPosition)
        {
            var itemsSourcePosition = GetItemsSourcePosition(viewPosition);

            if (itemsSourcePosition >= 0 && itemsSourcePosition < _itemsSource.Count())
                return _itemsSource.ElementAt(itemsSourcePosition);

            return null;
        }

        protected virtual int GetViewPosition(object item)
        {
            var itemsSourcePosition = _itemsSource.GetPosition(item);
            return GetViewPosition(itemsSourcePosition);
        }

        protected virtual int GetViewPosition(int itemsSourcePosition)
        {
            return itemsSourcePosition;
        }

        protected virtual int GetItemsSourcePosition(int viewPosition)
        {
            return viewPosition;
        }

        public int ItemTemplateId { get; set; }

        protected virtual void SetItemsSource(IEnumerable value)
        {
            if (Looper.MainLooper != Looper.MyLooper())
                Mvx.IoCProvider.Resolve<IMvxLog>().Error("ItemsSource property set on a worker thread. This leads to crash in the RecyclerView. It must be set only from the main thread.");

            if (ReferenceEquals(_itemsSource, value) && !ReloadOnAllItemsSourceSets)
                return;

            _subscription?.Dispose();
            _subscription = null;

            if (value != null && !(value is IList))
            {
                MvxBindingLog.Warning("Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");
            }

            if (value is INotifyCollectionChanged newObservable)
                _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);

            _itemsSource = value;
            NotifyDataSetChanged();
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(_subscription == null || _itemsSource == null) //Object disposed
                return;

            if (Looper.MainLooper == Looper.MyLooper())
                NotifyDataSetChanged(e);
            else
                Mvx.IoCProvider.Resolve<IMvxLog>().Error("ItemsSource collection content changed on a worker thread. This leads to crash in the RecyclerView as it will not be aware of changes immediatly and may get a deleted item or update an item with a bad item template. All changes must be synchronized on the main thread.");
        }

        public virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        NotifyItemRangeInserted(GetViewPosition(e.NewStartingIndex), e.NewItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        for (var i = 0; i < e.NewItems.Count; i++)
                            NotifyItemMoved(GetViewPosition(e.OldStartingIndex + i), GetViewPosition(e.NewStartingIndex + i));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        NotifyItemRangeChanged(GetViewPosition(e.NewStartingIndex), e.NewItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        NotifyItemRangeRemoved(GetViewPosition(e.OldStartingIndex), e.OldItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        NotifyDataSetChanged();
                        break;
                }
        }

        public event Action<MvxViewHolderBoundEventArgs> MvxViewHolderBound;

        protected virtual void OnMvxViewHolderBound(MvxViewHolderBoundEventArgs obj)
        {
            MvxViewHolderBound?.Invoke(obj);
        }

        /// <summary>
        /// Always called with disposing = false, as it is only disposed from java
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            //Mvx.IoCProvider.Resolve<IMvxLog>().Trace("MvxRecyclerAdapter Dispose");

            //if (disposing)
            {
                _subscription?.Dispose();
                _subscription = null;
                _itemClick = null;
                _itemLongClick = null;
                _itemsSource = null;
                _itemTemplateSelector = null;
            }

            base.Dispose(disposing);
        }
    }
}
