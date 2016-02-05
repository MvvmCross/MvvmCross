// MvxRecyclerAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Input;
using Android.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using MvvmCross.Binding;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.ExtensionMethods;

namespace MvvmCross.Droid.Support.V7.RecyclerView
{
    public class MvxRecyclerAdapter : Android.Support.V7.Widget.RecyclerView.Adapter, IMvxRecyclerAdapter
    {
        public event EventHandler DataSetChanged;

        private readonly IMvxAndroidBindingContext _bindingContext;

        private ICommand _itemClick, _itemLongClick;
        private IEnumerable _itemsSource;
        private IDisposable _subscription;
        private int _itemTemplateId;

        protected IMvxAndroidBindingContext BindingContext => _bindingContext;

        public MvxRecyclerAdapter() : this(MvxAndroidBindingContextHelpers.Current()) { }
        public MvxRecyclerAdapter(IMvxAndroidBindingContext bindingContext)
        {
            this._bindingContext = bindingContext;
        }

        public bool ReloadOnAllItemsSourceSets { get; set; }

        public ICommand ItemClick
        {
            get { return _itemClick; }
            set
            {
                if (ReferenceEquals(_itemClick, value))
                {
                    return;
                }

                if (_itemClick != null)
                {
                    MvxTrace.Warning("Changing ItemClick may cause inconsistencies where some items still call the old command.");
                }

                _itemClick = value;
            }
        }

        public ICommand ItemLongClick {
            get { return _itemLongClick; }
            set
            {
                if (ReferenceEquals(_itemLongClick, value))
                {
                    return;
                }

                if (_itemLongClick != null)
                {
                    MvxTrace.Warning("Changing ItemLongClick may cause inconsistencies where some items still call the old command.");
                }

                _itemLongClick = value;
            }
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set { SetItemsSource(value); }
        }

        public virtual int ItemTemplateId
        {
            get { return _itemTemplateId; }
            set
            {
                if (_itemTemplateId == value)
                {
                    return;
                }

                _itemTemplateId = value;

                // since the template has changed then let's force the list to redisplay by firing NotifyDataSetChanged()
                if (_itemsSource != null)
                {
                    NotifyAndRaiseDataSetChanged();
                }
            }
        }

        public override void OnViewAttachedToWindow(Java.Lang.Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnAttachedToWindow();
        }

        public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
        {
            base.OnViewDetachedFromWindow(holder);

            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnDetachedFromWindow();
        }

        public override Android.Support.V7.Widget.RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, _bindingContext.LayoutInflaterHolder);

            return new MvxRecyclerViewHolder(InflateViewForHolder(parent, viewType, itemBindingContext), itemBindingContext)
            {
                Click = ItemClick,
                LongClick = ItemLongClick
            };
        }

        protected virtual View InflateViewForHolder(ViewGroup parent, int viewType, IMvxAndroidBindingContext bindingContext)
        {
            return bindingContext.BindingInflate(this.ItemTemplateId, parent, false);
        }

        public override void OnBindViewHolder(Android.Support.V7.Widget.RecyclerView.ViewHolder holder, int position)
        {
            ((IMvxRecyclerViewHolder)holder).DataContext = _itemsSource.ElementAt(position);
        }

        public override int ItemCount => _itemsSource.Count();

        public virtual object GetItem(int position)
        {
            return _itemsSource.ElementAt(position);
        }

        protected virtual void SetItemsSource(IEnumerable value)
        {
            if (ReferenceEquals(_itemsSource, value) && !ReloadOnAllItemsSourceSets)
            {
                return;
            }

            if (_subscription != null)
            {
                _subscription.Dispose();
                _subscription = null;
            }

            _itemsSource = value;

            if (_itemsSource != null && !(_itemsSource is IList))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                    "Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");
            }

            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
            {
                _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);
            }

            NotifyAndRaiseDataSetChanged();
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
                        this.NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
                        this.RaiseDataSetChanged();
                        break;
                    case NotifyCollectionChangedAction.Move:
                        for (int i = 0; i < e.NewItems.Count; i++)
                        {
                            var oldItem = e.OldItems[i];
                            var newItem = e.NewItems[i];

                            this.NotifyItemMoved(this.ItemsSource.GetPosition(oldItem), this.ItemsSource.GetPosition(newItem));
                            this.RaiseDataSetChanged();
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        this.NotifyItemRangeChanged(e.NewStartingIndex, e.NewItems.Count);
                        this.RaiseDataSetChanged();
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        this.NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
                        this.RaiseDataSetChanged();
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        this.NotifyAndRaiseDataSetChanged();
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

        private void RaiseDataSetChanged()
        {
            var handler = DataSetChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }
        
        private void NotifyAndRaiseDataSetChanged()
        {
            this.RaiseDataSetChanged();
            this.NotifyDataSetChanged();
        }
    }
}
