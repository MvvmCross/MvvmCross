using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Droid.Support.V7.RecyclerView.Grouping.DataConverters
{
    internal class MvxGroupedItemsSourceProvider
    {
        private readonly ObservableCollection<object> _observableItemsSource = new ObservableCollection<object>();
        private readonly IList<IDisposable> _collectionChangedDisposables = new List<IDisposable>();
        private readonly Dictionary<MvxGroupedData, IDisposable> _groupedDataDisposables = new Dictionary<MvxGroupedData, IDisposable>();

        public ObservableCollection<object> Source => _observableItemsSource;

        public void Initialize(IEnumerable groupedItems, IMvxGroupedDataConverter groupedDataConverter)
        {
            DisposeOldSource();
            AddItems(groupedItems, groupedDataConverter);

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
                                    foreach (var disposables in _groupedDataDisposables.Values)
                                        disposables.Dispose();

                                    _groupedDataDisposables.Clear();
                                    break;
                                case NotifyCollectionChangedAction.Add:
                                    AddItems(args.NewItems, groupedDataConverter);
                                    break;
                                case NotifyCollectionChangedAction.Remove:
                                    foreach (var item in Enumerable.Cast<object>(args.OldItems))
                                    {
                                        var mvxGroupedData = groupedDataConverter.ConvertToMvxGroupedData(item);

                                        foreach (var childItem in mvxGroupedData.Items)
                                            _observableItemsSource.Remove(childItem);
                                        _observableItemsSource.Remove(mvxGroupedData);

                                        if (_groupedDataDisposables.ContainsKey(mvxGroupedData))
                                        {
                                            _groupedDataDisposables[mvxGroupedData].Dispose();
                                            _groupedDataDisposables.Remove(mvxGroupedData);
                                        }
                                    }
                                    break;
                                default:
                                    throw new NotImplementedException("No move/replace in Grouped Items yet...");
                            }
                        });
                _collectionChangedDisposables.Add(observableGroupsDisposeSubscription);
            }
        }

        private void DisposeOldSource()
        {
            _observableItemsSource.Clear();
            foreach (var disposables in _collectionChangedDisposables)
                disposables.Dispose();
            foreach (var disposable in _groupedDataDisposables.Values)
                disposable.Dispose();

            _groupedDataDisposables.Clear();
            _collectionChangedDisposables.Clear();
        }

        private void AddItems(IEnumerable groupedItems, IMvxGroupedDataConverter groupedDataConverter)
        {
            foreach (var mvxGroupable in groupedItems.Cast<object>().Select(groupedDataConverter.ConvertToMvxGroupedData))
            {
                if (!_observableItemsSource.Contains(mvxGroupable))
                    _observableItemsSource.Add(mvxGroupable);

                foreach (var child in mvxGroupable.Items.Cast<object>().ToList())
                {
                    if (!_observableItemsSource.Contains(child))
                        _observableItemsSource.Add(child);
                }


                var childNotifyCollectionChanged = mvxGroupable.Items as INotifyCollectionChanged;

                if (childNotifyCollectionChanged == null)
                    continue;

                if (!_groupedDataDisposables.ContainsKey(mvxGroupable))
                {
                    _groupedDataDisposables.Add(mvxGroupable, childNotifyCollectionChanged.WeakSubscribe((sender, args) =>
                    {
                        switch (args.Action)
                        {
                            case NotifyCollectionChangedAction.Add:
                                AddChildItems(mvxGroupable, args.NewItems);
                                break;
                            case NotifyCollectionChangedAction.Remove:
                                foreach (var itemToRemove in args.OldItems)
                                    _observableItemsSource.Remove(itemToRemove);
                                break;
                            case NotifyCollectionChangedAction.Reset:
                                ResetChildCollection(mvxGroupable);
                                break;
                            default:
                                throw new NotImplementedException("No move/replace in Grouped Items yet...");
                        }
                    }));
                }
            }
        }

        private void AddChildItems(MvxGroupedData toGroup, IEnumerable items)
        {
            var groupIndex = _observableItemsSource.IndexOf(toGroup);

            if (groupIndex == -1)
                return;

            foreach (var itemToAdd in items)
                _observableItemsSource.Insert(toGroup.Items.Count() + groupIndex, itemToAdd);
        }

        private void ResetChildCollection(MvxGroupedData ofGroupedData)
        {
            var groupIndex = _observableItemsSource.IndexOf(ofGroupedData);

            if (groupIndex == -1)
                return;

            var listToDelete = new List<object>();
            groupIndex++;

            for (int i = groupIndex; i < _observableItemsSource.Count && _observableItemsSource[i] is MvxGroupedData == false; ++i)
                listToDelete.Add(_observableItemsSource[groupIndex]);

            foreach (var itemToRemove in listToDelete)
                _observableItemsSource.Remove(itemToRemove);
        }
    }
}