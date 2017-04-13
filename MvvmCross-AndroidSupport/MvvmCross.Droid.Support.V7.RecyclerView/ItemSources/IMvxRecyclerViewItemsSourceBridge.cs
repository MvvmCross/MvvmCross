using System.Collections;
using System.Collections.Specialized;

namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemSources
{
    internal interface IMvxRecyclerViewItemsSourceBridge
    {
        IEnumerable GetItemsSource();

        void SetInternalItemsSource(IEnumerable itemsSourceEnumerable);

        int ItemsCount { get; }

        object GetItemAt(int position);
    }
}