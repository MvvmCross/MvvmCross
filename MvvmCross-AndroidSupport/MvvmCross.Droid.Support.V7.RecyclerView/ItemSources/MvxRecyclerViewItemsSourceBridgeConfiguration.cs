using System.Collections.Specialized;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping.DataConverters;

namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemSources
{
    internal class MvxRecyclerViewItemsSourceBridgeConfiguration
    {
        private readonly MvxRecyclerViewItemsSourceBridgeGroupingDecorator _itemsSourceBridgeGroupingDecorator;
        private readonly MvxRecyclerViewItemsSourceBridgeHeaderFooterDecorator _itemsSourceBridgeHeaderFooterDecorator;
        private readonly MvxRecyclerViewItemsSourceBridge _mvxRecyclerViewItemsSourceBridge;

        public MvxRecyclerViewItemsSourceBridgeConfiguration()
        {
            _mvxRecyclerViewItemsSourceBridge = new MvxRecyclerViewItemsSourceBridge();
            _itemsSourceBridgeGroupingDecorator =
                new MvxRecyclerViewItemsSourceBridgeGroupingDecorator(_mvxRecyclerViewItemsSourceBridge);
            _itemsSourceBridgeHeaderFooterDecorator =
                new MvxRecyclerViewItemsSourceBridgeHeaderFooterDecorator(_itemsSourceBridgeGroupingDecorator);
            _mvxRecyclerViewItemsSourceBridge.ItemsSourceCollectionChanged +=
                MvxRecyclerViewItemsSourceBridgeOnItemsSourceCollectionChanged;
        }

        public bool ReloadOnAllItemsSourceSets
        {
            get { return _mvxRecyclerViewItemsSourceBridge.ReloadOnAllItemsSourceSets; }
            set { _mvxRecyclerViewItemsSourceBridge.ReloadOnAllItemsSourceSets = value; }
        }

        public bool IsHeaderEnabled
        {
            get { return _itemsSourceBridgeHeaderFooterDecorator.IsHeaderEnabled; }
            set { _itemsSourceBridgeHeaderFooterDecorator.IsHeaderEnabled = value; }
        }

        public bool IsFooterEnabled
        {
            get { return _itemsSourceBridgeHeaderFooterDecorator.IsFooterEnabled; }
            set { _itemsSourceBridgeHeaderFooterDecorator.IsFooterEnabled = value; }
        }

        public IMvxRecyclerViewItemsSourceBridge ItemsSourceBridge => _itemsSourceBridgeHeaderFooterDecorator;

        public bool HidesHeaderIfEmpty
        {
            get { return _itemsSourceBridgeHeaderFooterDecorator.HidesHeaderIfEmpty; }
            set { _itemsSourceBridgeHeaderFooterDecorator.HidesHeaderIfEmpty = value; }
        }

        public bool HidesFooterIfEmpty
        {
            get { return _itemsSourceBridgeHeaderFooterDecorator.HidesFooterIfEmpty; }
            set { _itemsSourceBridgeHeaderFooterDecorator.HidesFooterIfEmpty = value; }
        }

        public IMvxGroupedDataConverter GroupedDataConverter
        {
            get { return _itemsSourceBridgeGroupingDecorator.GroupedDataConverter; }
            set { _itemsSourceBridgeGroupingDecorator.GroupedDataConverter = value; }
        }

        private void MvxRecyclerViewItemsSourceBridgeOnItemsSourceCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            ItemsSourceChanged?.Invoke(sender, notifyCollectionChangedEventArgs);
        }

        public event NotifyCollectionChangedEventHandler ItemsSourceChanged;
    }
}