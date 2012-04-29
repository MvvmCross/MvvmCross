#region Copyright
// <copyright file="MvxObservableCollection.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

// External credit:
// This file relies heavily on the Mono project - used under MIT license:
//  https://github.com/mono/mono/blob/master/mcs/class/System/System.Collections.ObjectModel/ObservableCollection.cs

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Cirrious.MvvmCross.Interfaces.ViewModels.Collections;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxObservableCollection<T> : Collection<T>, IMvxNotifyCollectionChanged, INotifyPropertyChanged
    {
        private class Reentrant : IDisposable
        {
            private int count = 0;

            public Reentrant()
            {
            }

            public void Enter()
            {
                count++;
            }

            public void Dispose()
            {
                count--;
            }

            public bool Busy
            {
                get { return count > 0; }
            }
        }

        private Reentrant reentrant = new Reentrant();

        public MvxObservableCollection()
        {
        }

        public MvxObservableCollection(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            foreach (var item in collection)
                Add(item);
        }

        public MvxObservableCollection(List<T> list)
            : base(list != null ? new List<T>(list) : null)
        {
        }

        public virtual event MvxNotifyCollectionChangedEventHandler CollectionChanged;

        public object NativeCollection { get; set; }

        protected virtual event PropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += value; }
            remove { this.PropertyChanged -= value; }
        }

        protected IDisposable BlockReentrancy()
        {
            reentrant.Enter();
            return reentrant;
        }

        protected void CheckReentrancy()
        {
            MvxNotifyCollectionChangedEventHandler eh = CollectionChanged;

            // Only have a problem if we have more than one event listener.
            if (reentrant.Busy && eh != null && eh.GetInvocationList().Length > 1)
                throw new InvalidOperationException("Cannot modify the collection while reentrancy is blocked.");
        }

        protected override void ClearItems()
        {
            CheckReentrancy();

            base.ClearItems();

            OnCollectionChanged(new MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction.Reset));
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        protected override void InsertItem(int index, T item)
        {
            CheckReentrancy();

            base.InsertItem(index, item);

            OnCollectionChanged(new MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction.Add, item, index));
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        public void Move(int oldIndex, int newIndex)
        {
            MoveItem(oldIndex, newIndex);
        }

        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            CheckReentrancy();

            T item = Items[oldIndex];
            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, item);

            OnCollectionChanged(new MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        protected virtual void OnCollectionChanged(MvxNotifyCollectionChangedEventArgs e)
        {
            MvxNotifyCollectionChangedEventHandler eh = CollectionChanged;

            if (eh != null)
            {
                // Make sure that the invocation is done before the collection changes,
                // Otherwise there's a chance of data corruption.
                using (BlockReentrancy())
                {
                    eh(this, e);
                }
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler eh = PropertyChanged;

            if (eh != null)
                eh(this, e);
        }

        protected override void RemoveItem(int index)
        {
            CheckReentrancy();

            T item = Items[index];

            base.RemoveItem(index);

            OnCollectionChanged(new MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction.Remove, item, index));
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        protected override void SetItem(int index, T item)
        {
            CheckReentrancy();

            T oldItem = Items[index];

            base.SetItem(index, item);

            OnCollectionChanged(new MvxNotifyCollectionChangedEventArgs(MvxNotifyCollectionChangedAction.Replace, item, oldItem, index));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }
    }
}