using System.Collections;
using System.Collections.Specialized;
using Android.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemSources.Data;

namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemSources
{
    internal class MvxRecyclerViewItemsSourceBridgeHeaderFooterDecorator : IMvxRecyclerViewItemsSourceBridge
    {
        private readonly IMvxRecyclerViewItemsSourceBridge _decoratedItemsSourceBridge;

        public MvxRecyclerViewItemsSourceBridgeHeaderFooterDecorator(IMvxRecyclerViewItemsSourceBridge decoratedItemsSourceBridge)
        {
            _decoratedItemsSourceBridge = decoratedItemsSourceBridge;
        }

        public IEnumerable GetItemsSource()
        {
            if (IsHeaderEnabled)
                yield return new MvxHeaderItemData() { ItemsSource = _decoratedItemsSourceBridge.GetItemsSource() };

            foreach (var item in _decoratedItemsSourceBridge.GetItemsSource())
                yield return item;

            if (IsFooterEnabled)
                yield return new MvxFooterItemData() { ItemsSource = _decoratedItemsSourceBridge.GetItemsSource() };
        }

        public void SetInternalItemsSource(IEnumerable itemsSourceEnumerable)
            => _decoratedItemsSourceBridge.SetInternalItemsSource(itemsSourceEnumerable);

        public int ItemsCount
        {
            get
            {
                var itemsCount = _decoratedItemsSourceBridge.ItemsCount;
               
                itemsCount += ShouldShowHeader ? 1 : 0;
                itemsCount += ShouldShowFooter ? 1 : 0;

                return itemsCount;
            }
        }

        public bool IsHeaderEnabled { get; internal set; }

        public bool IsFooterEnabled { get; internal set; }

        public bool HidesHeaderIfEmpty { get; internal set; } = true;

        public bool HidesFooterIfEmpty { get; internal set; } = true;

        private bool ShouldShowHeader => IsHeaderEnabled && (!HidesHeaderIfEmpty || _decoratedItemsSourceBridge.ItemsCount > 0);

        private bool ShouldShowFooter => IsFooterEnabled && (!HidesFooterIfEmpty || _decoratedItemsSourceBridge.ItemsCount > 0);

        public object GetItemAt(int position)
        {
            if (ShouldShowHeader && position == 0)
                return new MvxHeaderItemData() { ItemsSource = _decoratedItemsSourceBridge.GetItemsSource() };

            if (ShouldShowFooter && position == ItemsCount - 1)
                return new MvxFooterItemData() { ItemsSource = _decoratedItemsSourceBridge.GetItemsSource() };

            int orginalItemPosition = position;
            orginalItemPosition -= ShouldShowHeader ? 1 : 0;

            return _decoratedItemsSourceBridge.GetItemAt(orginalItemPosition);
        }
    }
}