using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Droid.Support.V7.RecyclerView.Grouping.DataConverters
{
    internal class MvxGroupedItemsSourceProvider
    {
        private readonly ObservableCollection<object> _observableItemsSource = new ObservableCollection<object>();
        private readonly IList<IDisposable> _collectionChangedDisposables = new List<IDisposable>();

        public ObservableCollection<object> Source => _observableItemsSource;

        public void Initialize(IEnumerable groupedItems, IMvxGroupedDataConverter groupedDataConverter)
        {
            _observableItemsSource.Clear();
            foreach (var disposables in _collectionChangedDisposables)
                disposables.Dispose();
            _collectionChangedDisposables.Clear();

            foreach (var mvxGroupable in groupedItems.Cast<object>().Select(groupedDataConverter.ConvertToMvxGroupedData))
            {
                _observableItemsSource.Add(mvxGroupable);
                foreach (var child in mvxGroupable.Items)
                    _observableItemsSource.Add(child);
            }

            var observableGroups = groupedItems as INotifyCollectionChanged;

            if (observableGroups != null)
            {
                var observableGroupsDisposeSubscription =
                    observableGroups.WeakSubscribe(
                        (sender, args) =>
                        {
                            switch (args.Action)
                            {
                                case NotifyCollectionChangedAction.Reset:
                                    _observableItemsSource.Clear();
                                    break;
                                case NotifyCollectionChangedAction.Add:
                                    foreach (var item in Enumerable.Cast<object>(args.NewItems))
                                        _observableItemsSource.Add(groupedDataConverter.ConvertToMvxGroupedData(item));
                                    break;
                                case NotifyCollectionChangedAction.Remove:
                                    foreach (var item in Enumerable.Cast<object>(args.OldItems))
                                    {
                                        var mvxGroupedData = groupedDataConverter.ConvertToMvxGroupedData(item);
                                        _observableItemsSource.Remove(mvxGroupedData);
                                        foreach (var childItem in mvxGroupedData.Items)
                                            _observableItemsSource.Remove(childItem);
                                    }
                                    break;
                                default:
                                    throw new InvalidOperationException("No move/replace in Grouped Items yet...");
                            }
                        });
                _collectionChangedDisposables.Add(observableGroupsDisposeSubscription);
            }
        }

    }
}