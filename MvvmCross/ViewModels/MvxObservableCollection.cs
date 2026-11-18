// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Base;
using MvvmCross.Hosting;


namespace MvvmCross.ViewModels
{
    public class MvxObservableCollection<T>
        : ObservableCollection<T>
    {
        protected struct SuppressEventsDisposable : IDisposable
        {
            private readonly MvxObservableCollection<T> _collection;

            public SuppressEventsDisposable(MvxObservableCollection<T> collection)
            {
                _collection = collection;
                ++collection._suppressEvents;
            }

            public void Dispose()
            {
                --_collection._suppressEvents;
            }
        }

        private int _suppressEvents;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvxObservableCollection{T}"/> class.
        /// </summary>
        public MvxObservableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MvxObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="items">The collection from which the items are copied.</param>
        public MvxObservableCollection(IEnumerable<T> items)
            : base(items)
        {
        }

        protected virtual SuppressEventsDisposable SuppressEvents()
        {
            return new SuppressEventsDisposable(this);
        }

        public virtual bool EventsAreSuppressed
        {
            get { return _suppressEvents > 0; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged"/> event with the provided event data.
        /// </summary>
        /// <param name="e">The event data to report in the event.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!EventsAreSuppressed)
            {
                InvokeOnMainThread(() => base.OnCollectionChanged(e));
            }
        }

        /// <summary>
        /// Adds the specified items collection to the current <see cref="MvxObservableCollection{T}"/> instance.
        /// </summary>
        /// <param name="items">The collection of items to be added.</param>
        /// <exception cref="ArgumentNullException">The items list is null.</exception>
        public virtual void AddRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int startingIndex = this.Items.Count;
            var itemsList = items.ToList();
            using (SuppressEvents())
            {
                foreach (var item in itemsList)
                {
                    Add(item);
                }
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, itemsList, startingIndex));
        }

        /// <summary>
        /// Inserts the specified items collection in the current <see cref="MvxObservableCollection{T}"/> instance at the specified index.
        /// </summary>
        /// <param name="index">The position where the collection of items should be inserted at.</param>
        /// <param name="items">The collection of items to be inserted.</param>
        /// <exception cref="ArgumentNullException">The items list is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Index incorrect.</exception>
        public virtual void InsertRange(int index, IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            int currentIndex = index;
            var itemsList = items.ToList();
            using (SuppressEvents())
            {
                foreach (var item in itemsList)
                {
                    InsertItem(currentIndex, item);
                    currentIndex++;
                }
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, itemsList, index));
        }

        /// <summary>
        /// Replaces the current <see cref="MvxObservableCollection{T}"/> instance items with the ones specified in the items collection, raising a single <see cref="NotifyCollectionChangedAction.Reset"/> event.
        /// </summary>
        /// <param name="items">The collection of items that will replace the current <see cref="MvxObservableCollection{T}"/> instance items.</param>
        /// <exception cref="ArgumentNullException">The items list is null.</exception>
        public virtual void ReplaceWith(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            using (SuppressEvents())
            {
                Clear();
                AddRange(items);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public virtual void ReplaceRange(IEnumerable<T> items, int firstIndex, int oldSize)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var newItems = ToSpan(items);
            var startIndex = firstIndex;
            var replacedCount = Math.Min(newItems.Length, oldSize);

            // The event args need the replaced items after they have been
            // overwritten, so snapshot just that segment into an array.
            var oldItems = new T[replacedCount];
            for (var i = 0; i < replacedCount; i++)
            {
                oldItems[i] = this[startIndex + i];
            }

            using (SuppressEvents())
            {
                var lastIndex = firstIndex + oldSize - 1;

                // If there are more items in the previous list, remove them.
                while (firstIndex + newItems.Length <= lastIndex)
                {
                    RemoveAt(lastIndex--);
                }

                foreach (var item in newItems)
                {
                    if (firstIndex <= lastIndex)
                        SetItem(firstIndex++, item);
                    else
                        Insert(firstIndex++, item);
                }
            }

            if (replacedCount > 0)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    newItems.Slice(0, replacedCount).ToArray(),
                    oldItems,
                    startIndex));
            }

            if (newItems.Length > replacedCount)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    newItems.Slice(replacedCount).ToArray(),
                    startIndex + replacedCount));
            }
        }


        /// <summary>
        /// Switches the current <see cref="MvxObservableCollection{T}"/> instance items with the ones specified in the items collection, raising the minimum required change events.
        /// </summary>
        /// <param name="items">The collection from which the items are copied.</param>
        /// <exception cref="ArgumentNullException">The items list is null.</exception>
        public virtual void SwitchTo(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var itemIndex = 0;
            var count = Count;

            foreach (var item in items)
            {
                if (itemIndex >= count)
                {
                    Add(item);
                }
                else if (!Equals(this[itemIndex], item))
                {
                    this[itemIndex] = item;
                }

                itemIndex++;
            }

            while (count > itemIndex)
            {
                RemoveAt(--count);
            }
        }

        /// <summary>
        /// Removes the current <see cref="MvxObservableCollection{T}"/> instance items of the ones specified in the items collection, raising the minimum required change events.
        /// </summary>
        /// <param name="items">The collection which items will be removed.</param>
        /// <exception cref="ArgumentNullException">The items list is null.</exception>
        public virtual void RemoveItems(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var itemsToRemove = ToSpan(items);
            var removedItems = new List<(T Item, int Index)>();

            using (SuppressEvents())
            {
                foreach (var item in itemsToRemove)
                {
                    var index = IndexOf(item);
                    if (index < 0)
                    {
                        continue;
                    }

                    removedItems.Add((item, index));
                    Remove(item);
                }
            }

            foreach (var (removedItem, index) in removedItems)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove, removedItem, index));
            }
        }



        /// <summary>
        /// Removes the current <see cref="MvxObservableCollection{T}"/> instance items of the ones specified in the range, raising the minimum required change events.
        /// </summary>
        /// <param name="start">The start index.</param>
        /// <param name="count">The count of items to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Start index or count incorrect.</exception>
        public virtual void RemoveRange(int start, int count)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var end = start + count - 1;

            if (end < 0 || end > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var removedItems = new List<T>(count);
            for (var i = start; i <= end; i++)
            {
                removedItems.Add(this[i]);
            }

            using (SuppressEvents())
            {
                for (var i = end; i >= start; i--)
                {
                    RemoveAt(i);
                }
            }

            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems, start));
        }

        // Returns a span over the items without copying when the source is
        // an array or a list. Any other IEnumerable<T> has to be enumerated
        // to be indexed into, so it is materialized into an array once.
        private static ReadOnlySpan<T> ToSpan(IEnumerable<T> items)
        {
            if (items is T[] itemsArray)
            {
                return itemsArray;
            }

            if (items is List<T> list)
            {
                return CollectionsMarshal.AsSpan(list);
            }

            return items.ToArray();
        }

        private IMvxMainThreadAsyncDispatcher _dispatcher;

        protected virtual Task InvokeOnMainThread(Action action)
        {
            if (_dispatcher != null)
                return _dispatcher.ExecuteOnMainThreadAsync(action);

            _dispatcher = MvxHost.Current?.Services.GetService<IMvxMainThreadAsyncDispatcher>();
            if (_dispatcher == null)
                return Task.CompletedTask;

            return _dispatcher.ExecuteOnMainThreadAsync(action);
        }

        protected async override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            await InvokeOnMainThread(() => base.OnPropertyChanged(e));
        }
    }
}
