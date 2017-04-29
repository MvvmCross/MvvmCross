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
        private readonly IList<IDisposable> _collectionChangedDisposables = new List<IDisposable>();

        public ObservableCollection<object> Source { get; } = new ObservableCollection<object>();

        public void Initialize(IEnumerable groupedItems, IMvxGroupedDataConverter groupedDataConverter)
        {
            Source.Clear();
            foreach (var disposables in _collectionChangedDisposables)
                disposables.Dispose();
            _collectionChangedDisposables.Clear();

            foreach (var mvxGroupable in groupedItems.Cast<object>()
                .Select(groupedDataConverter.ConvertToMvxGroupedData))
            {
                Source.Add(mvxGroupable);
                foreach (var child in mvxGroupable.Items)
                    Source.Add(child);
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
                                    Source.Clear();
                                    break;
                                case NotifyCollectionChangedAction.Add:
                                    foreach (var item in args.NewItems.Cast<object>())
                                        Source.Add(groupedDataConverter.ConvertToMvxGroupedData(item));
                                    break;
                                case NotifyCollectionChangedAction.Remove:
                                    foreach (var item in args.OldItems.Cast<object>())
                                    {
                                        var mvxGroupedData = groupedDataConverter.ConvertToMvxGroupedData(item);
                                        Source.Remove(mvxGroupedData);
                                        foreach (var childItem in mvxGroupedData.Items)
                                            Source.Remove(childItem);
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