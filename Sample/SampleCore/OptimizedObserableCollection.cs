using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace SampleCore
{
    /// <summary>
    /// Optimized version of the <see cref="OptimizedObservableCollection{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class OptimizedObservableCollection<T> : ObservableCollection<T>
    {
        private bool _suppressEvents;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="OptimizedObservableCollection{T}.CollectionChanged"/> events are raised.
        /// </summary>
        /// <value>true if the <see cref="OptimizedObservableCollection{T}.CollectionChanged"/> events are raised; otherwise, false.</value>
        public bool SuppressEvents
        {
            get
            {
                return _suppressEvents;
            }
            set
            {
                if (_suppressEvents != value)
                {
                    _suppressEvents = value;

                    OnPropertyChanged(new PropertyChangedEventArgs("SuppressEvents"));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimizedObservableCollection{T}"/> class.
        /// </summary>
        public OptimizedObservableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimizedObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="items">The collection from which the items are copied.</param>
        public OptimizedObservableCollection(IEnumerable<T> items)
            : base(items)
        {
        }

        /// <summary>
        /// Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged"/> event with the provided event data.
        /// </summary>
        /// <param name="e">The event data to report in the event.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!SuppressEvents)
            {
                base.OnCollectionChanged(e);
            }
        }

        /// <summary>
        /// Adds the specified items collection to the current <see cref="OptimizedObservableCollection{T}"/> instance.
        /// </summary>
        /// <param name="items">The collection from which the items are copied.</param>
        /// <exception cref="ArgumentNullException">The items list is null.</exception>
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            try
            {
                SuppressEvents = true;

                foreach (var item in items)
                {
                    Add(item);
                }
            }
            finally
            {
                SuppressEvents = false;

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Replaces the current <see cref="OptimizedObservableCollection{T}"/> instance items with the ones specified in the items collection, raising a single <see cref="NotifyCollectionChangedAction.Reset"/> event.
        /// </summary>
        /// <param name="items">The collection from which the items are copied.</param>
        /// <exception cref="ArgumentNullException">The items list is null.</exception>
        public void ReplaceWith(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            SuppressEvents = true;

            Clear();

            AddRange(items);
        }

        public void ReplaceRange(IEnumerable<T> items, int firstIndex, int oldSize)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            try
            {
                SuppressEvents = true;

                var lastIndex = firstIndex + oldSize - 1;

                // If there are more items in the previous list, remove them.
                while (firstIndex + items.Count() <= lastIndex)
                {
                    RemoveAt(lastIndex--);
                }

                foreach (var item in items)
                {
                    if (firstIndex <= lastIndex)
                        SetItem(firstIndex++, item);
                    else
                        Insert(firstIndex++, item);
                }
            }
            finally
            {
                SuppressEvents = false;

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Switches the current <see cref="OptimizedObservableCollection{T}"/> instance items with the ones specified in the items collection, raising the minimum required change events.
        /// </summary>
        /// <param name="items">The collection from which the items are copied.</param>
        /// <exception cref="ArgumentNullException">The items list is null.</exception>
        public void SwitchTo(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
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
                this.RemoveAt(--count);
            }
        }
    }
}
