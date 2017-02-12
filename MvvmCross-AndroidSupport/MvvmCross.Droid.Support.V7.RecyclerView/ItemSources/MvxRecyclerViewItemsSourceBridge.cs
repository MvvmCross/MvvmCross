using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using MvvmCross.Binding;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemSources
{
    internal class MvxRecyclerViewItemsSourceBridge : IMvxRecyclerViewItemsSourceBridge
    {
        private MvxNotifyCollectionChangedEventSubscription _subscription;
        private IEnumerable _itemsSource = Enumerable.Empty<object>();

        public bool ReloadOnAllItemsSourceSets { get; set; }

        public void SetInternalItemsSource(IEnumerable itemsSourceEnumerable)
        {
            if (ReferenceEquals(_itemsSource, itemsSourceEnumerable) && !ReloadOnAllItemsSourceSets)
                return;

            _subscription?.Dispose();
            _subscription = null;

			_itemsSource = itemsSourceEnumerable ?? Enumerable.Empty<object>();

            if (_itemsSource != null && !(_itemsSource is IList))
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                    "Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");

            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
                _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);
            
            OnItemsSourceCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        public IEnumerable GetItemsSource()
            => _itemsSource;

        public object GetItemAt(int position)
            => _itemsSource.ElementAt(position);

        public int ItemsCount => _itemsSource.Count();

        public event NotifyCollectionChangedEventHandler ItemsSourceCollectionChanged;

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => ItemsSourceCollectionChanged?.Invoke(sender, e);
    }
}