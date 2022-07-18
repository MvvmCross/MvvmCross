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
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Extensions;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using MvvmCross.DroidX.RecyclerView.Model;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.WeakSubscription;
using Object = Java.Lang.Object;
using RecyclerViewAdapter = AndroidX.RecyclerView.Widget.RecyclerView.Adapter;
using ViewHolder = AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder;

namespace MvvmCross.DroidX.RecyclerView
{
#nullable enable
    [Register("mvvmcross.droidx.recyclerview.MvxRecyclerAdapter")]
    public class MvxRecyclerAdapter
        : RecyclerViewAdapter, IMvxRecyclerAdapter, IMvxRecyclerAdapterBindableHolder
    {
        public event Action<MvxViewHolderBoundEventArgs>? MvxViewHolderBound;

        private ICommand? _itemClick, _itemLongClick;
        private IEnumerable? _itemsSource;
        private IDisposable? _subscription;
        private IMvxTemplateSelector? _itemTemplateSelector;

        protected IMvxAndroidBindingContext? BindingContext { get; }

        public bool ReloadOnAllItemsSourceSets { get; set; }

        public int ItemTemplateId { get; set; }

        public MvxRecyclerAdapter()
            : this(null)
        {
        }

        public MvxRecyclerAdapter(IMvxAndroidBindingContext? bindingContext)
        {
            BindingContext = bindingContext ?? MvxAndroidBindingContextHelpers.Current();
        }

        [Android.Runtime.Preserve(Conditional = true)]
        protected MvxRecyclerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        /// <summary>
        /// Get or set the <see cref="ICommand"/> to trigger when an item was clicked.
        /// </summary>
        [MvxSetToNullAfterBinding]
        public ICommand? ItemClick
        {
            get => _itemClick;
            set
            {
                if (ReferenceEquals(_itemClick, value))
                    return;

                if (_itemClick != null && value != null)
                    MvxAndroidLog.Instance.Log(LogLevel.Warning, "Changing ItemClick may cause inconsistencies where some items still call the old command.");

                _itemClick = value;
            }
        }

        /// <summary>
        /// Get or set the <see cref="ICommand"/> to trigger when an item was long clicked.
        /// </summary>
        [MvxSetToNullAfterBinding]
        public ICommand? ItemLongClick
        {
            get => _itemLongClick;
            set
            {
                if (ReferenceEquals(_itemLongClick, value))
                    return;

                if (_itemLongClick != null && value != null)
                    MvxAndroidLog.Instance.Log(LogLevel.Warning, "Changing ItemLongClick may cause inconsistencies where some items still call the old command.");

                _itemLongClick = value;
            }
        }

        /// <summary>
        /// <para>Get or set the ItemSource to use in this adapter.</para>
        /// <para>
        /// It is recommended to use a type inheriting from <see cref="IList"/>, such as
        /// <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>,
        /// <see cref="MvvmCross.ViewModels.MvxObservableCollection{T}"/> or
        /// <see cref="System.Collections.Generic.List{T}"/>.
        /// </para>
        /// </summary>
        [MvxSetToNullAfterBinding]
        public virtual IEnumerable? ItemsSource
        {
            get => _itemsSource;
            set => SetItemsSource(value);
        }

        /// <summary>
        /// <para>Get or set the ItemTemplateSelector.</para>
        /// </summary>
        [MvxSetToNullAfterBinding]
        public virtual IMvxTemplateSelector? ItemTemplateSelector
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

            if (holder is IMvxRecyclerViewHolder mvxViewHolder)
            {
                mvxViewHolder.OnAttachedToWindow();
            }
        }

        public override void OnViewDetachedFromWindow(Object holder)
        {
            if (holder is IMvxRecyclerViewHolder mvxViewHolder)
            {
                mvxViewHolder.OnDetachedFromWindow();
            }

            base.OnViewDetachedFromWindow(holder);
        }

        public override int GetItemViewType(int position)
        {
            if (ItemTemplateSelector == null)
                throw new InvalidOperationException("ItemTemplateSelector is null or not the default template selector");

            var itemAtPosition = GetItem(position);
            var viewTypeIndex = ItemTemplateSelector.GetItemViewType(itemAtPosition);
            var viewType = ItemTemplateSelector.GetItemLayoutId(viewTypeIndex);
            return viewType;
        }

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent), "parent is null, cannot get Android Context.");

            if (BindingContext == null)
                throw new InvalidOperationException("BindingContext is null. Cannot inflate view for ViewHolder");

            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
            var view = InflateViewForHolder(parent, viewType, itemBindingContext);
            var viewHolder = new MvxRecyclerViewHolder(view, itemBindingContext)
            {
                Id = viewType
            };

            return viewHolder;
        }

        /// <summary>
        /// Inflate the View for a ViewHolder
        /// </summary>
        /// <param name="parent">Parent <see cref="ViewGroup"/> the View gets it layout parameters from</param>
        /// <param name="viewType"><para>This is the layout ID to inflate.</para>
        /// <para>This will typically come from the <see cref="ItemTemplateSelector"/> assigned to this instance.</para></param>
        /// <param name="bindingContext">The <see cref="IMvxAndroidBindingContext"/> used for binding of the inflated layout.</param>
        /// <returns>Returns a <see cref="View"/> with the view of the ViewHolder.</returns>
        protected virtual View InflateViewForHolder(ViewGroup parent, int viewType, IMvxAndroidBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext), "BindingContext is null. Cannot inflate view");

            return bindingContext.BindingInflate(viewType, parent, false);
        }

        public override void OnBindViewHolder(ViewHolder holder, int position)
        {
            var dataContext = GetItem(position);
            if (holder is IMvxRecyclerViewHolder viewHolder)
            {
                viewHolder.DataContext = dataContext;

                CheckIfSimpleListItem(holder, viewHolder, dataContext);

                AttachClickListeners(viewHolder);
            }

            OnMvxViewHolderBound(new MvxViewHolderBoundEventArgs(position, dataContext, holder));
        }

        public override void OnViewRecycled(Object holder)
        {
            if (holder is IMvxRecyclerViewHolder viewHolder)
            {
                viewHolder.Click -= OnItemViewClick;
                viewHolder.LongClick -= OnItemViewLongClick;
                viewHolder.OnViewRecycled();
            }
        }

        public override void OnDetachedFromRecyclerView(AndroidX.RecyclerView.Widget.RecyclerView recyclerView)
        {
            base.OnDetachedFromRecyclerView(recyclerView);
            Clean(false);
        }

        /// <summary>
        /// By default, force recycling a view if it has animations
        /// </summary>
        public override bool OnFailedToRecycleView(Object holder) => true;

        protected virtual void OnItemViewClick(object? sender, EventArgs? e)
        {
            if (sender is IMvxRecyclerViewHolder holder)
            {
                ExecuteCommandOnItem(ItemClick, holder.DataContext);
            }
        }

        protected virtual void OnItemViewLongClick(object? sender, EventArgs? e)
        {
            if (sender is IMvxRecyclerViewHolder holder)
            {
                ExecuteCommandOnItem(ItemLongClick, holder.DataContext);
            }
        }

        protected virtual void ExecuteCommandOnItem(ICommand? command, object itemDataContext)
        {
            if (command?.CanExecute(itemDataContext) == true && itemDataContext != null)
            {
                command.Execute(itemDataContext);
            }
        }

        public override int ItemCount => _itemsSource?.Count() ?? 0;

        /// <summary>
        /// Get item from <see cref="ItemsSource"/> at provided <paramref name="viewPosition"/>.
        /// </summary>
        /// <param name="viewPosition">Position to get item from</param>
        /// <returns>Returns object at given <paramref name="viewPosition"/></returns>
        public virtual object? GetItem(int viewPosition)
        {
            var itemsSourcePosition = GetItemsSourcePosition(viewPosition);

            //Do not check if viewPosition is in the range for non IList enumerables. Ie: do not call Count() on the list, as it triggers a full enumeration and kills performance for a streaming source.
            if (ItemsSource is IList items)
            {
                if (itemsSourcePosition >= 0 && itemsSourcePosition < items.Count)
                    return items[itemsSourcePosition];
                MvxAndroidLog.Instance.Log(LogLevel.Error,
                    "MvxRecyclerView GetItem index out of range. viewPosition: {ViewPosition}, itemsSourcePosition: {ItemsSourcePosition}, itemCount: {ItemsSourceCount}",
                    viewPosition, itemsSourcePosition, _itemsSource.Count());
                //We should trigger an exception instead of hiding it here, as it means you have bugs in your code.
                return null;
            }

            //May crash if itemsSourcePosition is out or range. Which should never happen anyway, except when you have bugs in your code.
            return _itemsSource?.ElementAt(itemsSourcePosition);
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

        protected virtual void SetItemsSource(IEnumerable? value)
        {
            if (Looper.MainLooper != Looper.MyLooper())
            {
                MvxAndroidLog.Instance.Log(LogLevel.Error,
                    "ItemsSource property set on a worker thread. This leads to crash in the RecyclerView. It must be set only from the main thread.");
            }

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
            if (_subscription == null || _itemsSource == null) //Object disposed
                return;

            if (Looper.MainLooper == Looper.MyLooper())
            {
                NotifyDataSetChanged(e);
            }
            else
            {
                MvxAndroidLog.Instance.Log(LogLevel.Error,
                    "ItemsSource collection content changed on a worker thread." +
                    "This leads to crash in the RecyclerView as it will not be aware of changes" +
                    "immediatly and may get a deleted item or update an item with a bad item template." +
                    "All changes must be synchronized on the main thread.");
            }
        }

        public virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e == null)
            {
                NotifyDataSetChanged();
                return;
            }

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

        protected virtual void OnMvxViewHolderBound(MvxViewHolderBoundEventArgs obj)
        {
            MvxViewHolderBound?.Invoke(obj);
        }

        private void AttachClickListeners(IMvxRecyclerViewHolder viewHolder)
        {
            viewHolder.Click -= OnItemViewClick;
            viewHolder.LongClick -= OnItemViewLongClick;
            viewHolder.Click += OnItemViewClick;
            viewHolder.LongClick += OnItemViewLongClick;
        }

        private static void CheckIfSimpleListItem(
            ViewHolder holder, IMvxRecyclerViewHolder viewHolder, object? dataContext)
        {
            if (viewHolder.Id == global::Android.Resource.Layout.SimpleListItem1)
            {
                ((TextView)holder.ItemView).Text = dataContext?.ToString();
            }
        }

        private void Clean(bool disposing)
        {
            if (disposing)
            {
                _subscription?.Dispose();
                _subscription = null;
                _itemClick = null;
                _itemLongClick = null;
                _itemsSource = null;
                _itemTemplateSelector = null;
            }
        }

        /// <summary>
        /// Always called with disposing = false, as it is only disposed from java
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            Clean(true);
            base.Dispose(disposing);
        }
    }
#nullable restore
}
