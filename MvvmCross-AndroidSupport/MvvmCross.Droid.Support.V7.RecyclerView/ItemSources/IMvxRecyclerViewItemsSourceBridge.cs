using System.Collections;

namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemSources
{
    internal interface IMvxRecyclerViewItemsSourceBridge
    {
        int ItemsCount { get; }
        IEnumerable GetItemsSource();

        void SetInternalItemsSource(IEnumerable itemsSourceEnumerable);

        object GetItemAt(int position);
    }
}