using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping;
using MvvmCross.Droid.Support.V7.RecyclerView.Grouping.DataConverters;

namespace MvvmCross.Droid.Support.V7.RecyclerView.ItemSources
{
    internal class MvxRecyclerViewItemsSourceBridgeGroupingDecorator : IMvxRecyclerViewItemsSourceBridge
    {
        private readonly IMvxRecyclerViewItemsSourceBridge _decoratedItemsSourceBridge;
        private readonly MvxGroupedItemsSourceProvider _groupedItemsConverter;

        public MvxRecyclerViewItemsSourceBridgeGroupingDecorator(
            IMvxRecyclerViewItemsSourceBridge decoratedItemsSourceBridge)
        {
            _decoratedItemsSourceBridge = decoratedItemsSourceBridge;
            _groupedItemsConverter = new MvxGroupedItemsSourceProvider();
        }

        public IMvxGroupedDataConverter GroupedDataConverter { get; set; }

        public IEnumerable GetItemsSource()
        {
            var baseItemsSource = _decoratedItemsSourceBridge.GetItemsSource() ?? Enumerable.Empty<object>();

            if (!IsGenericEnumerable(baseItemsSource))
            {
                foreach (var item in baseItemsSource)
                    yield return item;
                yield break;
            }

            var genericEnumerableArgumentType = GetEnumerableGenericArgumentType(baseItemsSource);

            if (genericEnumerableArgumentType != typeof(MvxGroupedData) && GroupedDataConverter == null) // sorry, no grouping support!
            {
                foreach (var item in baseItemsSource)
                    yield return item;
                yield break;
            }

            IMvxGroupedDataConverter groupedDataConverter =
                genericEnumerableArgumentType == typeof(MvxGroupedData) ? new MvxDefaultGroupedDataConverter() : GroupedDataConverter;
            _groupedItemsConverter.Initialize(baseItemsSource, groupedDataConverter);

            foreach (var groupedItemRow in _groupedItemsConverter.Source)
                yield return groupedItemRow;
        }

        public void SetInternalItemsSource(IEnumerable itemsSourceEnumerable)
        {
            _decoratedItemsSourceBridge.SetInternalItemsSource(itemsSourceEnumerable);
        }

        public int ItemsCount => GetItemsSource().Count();

        public object GetItemAt(int position) => GetItemsSource().ElementAt(position);

        private bool IsGenericEnumerable(IEnumerable enumerable)
        {
            var enumerableType = enumerable.GetType();
            return enumerableType.IsGenericType;
        }

        private Type GetEnumerableGenericArgumentType(IEnumerable enumerable)
        {
            var enumerableType = enumerable.GetType();
            var genericArguments = enumerableType.GetGenericArguments();
            if (genericArguments.Any())
                return genericArguments[0];

            return typeof(void);
        }
    }
}