#region Copyright
// <copyright file="MvxObservableCollectionConverter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.ViewModels.Collections;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.WindowsPhone.Platform.Converters
{
    public class MvxObservableCollectionConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var toBeWrapped = value as IMvxNotifyCollectionChanged;
            if (toBeWrapped == null)
            {
                throw new ArgumentException("value must support IMvxNotifyCollectionChanged", "value");
            }

            if (toBeWrapped.NativeCollection == null)
            {
                toBeWrapped.NativeCollection = new Wrapper(toBeWrapped);
            }

            if (!(toBeWrapped.NativeCollection is Wrapper))
            {
                throw new MvxException("Unexpected Native Wrapper");    
            }

            return toBeWrapped.NativeCollection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private class Wrapper
            : IList
            , INotifyCollectionChanged
            , INotifyPropertyChanged
        {
            private readonly object _wrapped;

            public Wrapper(IMvxNotifyCollectionChanged wrapped)
            {
                if (!(wrapped is IList))
                {
                    throw new ArgumentException("wrapped must support IList", "wrapped");
                }

                if (!(wrapped is INotifyPropertyChanged))
                {
                    throw new ArgumentException("wrapped must support INotifyPropertyChanged", "wrapped");
                }

                _wrapped = wrapped;
            }

            #region Implementation of IEnumerable

            public IEnumerator GetEnumerator()
            {
                return (_wrapped as IEnumerable).GetEnumerator();
            }

            #endregion

            #region Implementation of ICollection

            public void CopyTo(Array array, int index)
            {
                (_wrapped as ICollection).CopyTo(array, index);
            }

            public int Count
            {
                get { return (_wrapped as ICollection).Count; }
            }

            public object SyncRoot
            {
                get { return (_wrapped as ICollection).SyncRoot; }
            }

            public bool IsSynchronized
            {
                get { return (_wrapped as ICollection).IsSynchronized; }
            }

            #endregion

            #region Implementation of IList

            public int Add(object value)
            {
                return (_wrapped as IList).Add(value);
            }

            public bool Contains(object value)
            {
                return (_wrapped as IList).Contains(value);
            }

            public void Clear()
            {
                (_wrapped as IList).Clear();
            }

            public int IndexOf(object value)
            {
                return (_wrapped as IList).IndexOf(value);
            }

            public void Insert(int index, object value)
            {
                (_wrapped as IList).Insert(index, value);
            }

            public void Remove(object value)
            {
                (_wrapped as IList).Remove(value);
            }

            public void RemoveAt(int index)
            {
                (_wrapped as IList).Add(index);
            }

            public object this[int index]
            {
                get
                {
                    return (_wrapped as IList)[index];
                }
                set { (_wrapped as IList)[index] = value; }
            }

            public bool IsReadOnly
            {
                get { return (_wrapped as IList).IsReadOnly; }
            }

            public bool IsFixedSize
            {
                get { return (_wrapped as IList).IsFixedSize; }
            }

            #endregion

            #region Implementation of INotifyPropertyChanged

            private int _propertyChangedSubscribers;
            private event PropertyChangedEventHandler _propertyChanged;
            public event PropertyChangedEventHandler PropertyChanged
            {
                add
                {
                    _propertyChanged += value;
                    _propertyChangedSubscribers++;
                    if (_propertyChangedSubscribers == 1)
                    {
                        (_wrapped as INotifyPropertyChanged).PropertyChanged += PropertyChangedOnPropertyChanged;
                    }
                }
                remove
                {
                    _propertyChanged -= value;
                    _propertyChangedSubscribers--;
                    if (_propertyChangedSubscribers == 0)
                    {
                        (_wrapped as INotifyPropertyChanged).PropertyChanged -= PropertyChangedOnPropertyChanged;
                    }
                }
            }

            private void PropertyChangedOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
            {
                var handler = _propertyChanged;
                if (handler != null)
                {
                    handler(this, propertyChangedEventArgs);
                }
            }

            #endregion

            #region Implementation of INotifyCollectionChanged

            private int _countCollectionChangedSubscribers;
            public event NotifyCollectionChangedEventHandler _collectionChanged;
            public event NotifyCollectionChangedEventHandler CollectionChanged
            {
                add
                {
                    _collectionChanged += value;
                    _countCollectionChangedSubscribers++;
                    if (_countCollectionChangedSubscribers == 1)
                    {
                        (_wrapped as IMvxNotifyCollectionChanged).CollectionChanged += CollectionChangedOnCollectionChanged;
                    }
                }
                remove
                {
                    _collectionChanged -= value;
                    _countCollectionChangedSubscribers--;
                    if (_countCollectionChangedSubscribers == 0)
                    {
                        (_wrapped as IMvxNotifyCollectionChanged).CollectionChanged -= CollectionChangedOnCollectionChanged;
                    }
                }
            }

            private void CollectionChangedOnCollectionChanged(object sender, MvxNotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
            {
                var handler = _collectionChanged;
                if (handler != null)
                {
                    handler(this, ToNative(notifyCollectionChangedEventArgs));
                }
            }

            private NotifyCollectionChangedEventArgs ToNative(MvxNotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
            {
                switch (notifyCollectionChangedEventArgs.Action)
                {
                    case MvxNotifyCollectionChangedAction.Add:
                        if (notifyCollectionChangedEventArgs.NewItems.Count == 1)
                        {
                            return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, notifyCollectionChangedEventArgs.NewItems[0], notifyCollectionChangedEventArgs.NewStartingIndex);
                        }
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

                    case MvxNotifyCollectionChangedAction.Remove:
                        if (notifyCollectionChangedEventArgs.OldItems.Count == 1)
                        {
                            return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, notifyCollectionChangedEventArgs.OldItems[0], notifyCollectionChangedEventArgs.OldStartingIndex);
                        }
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

                    case MvxNotifyCollectionChangedAction.Replace:
                        if (notifyCollectionChangedEventArgs.NewItems.Count == 1
                            && notifyCollectionChangedEventArgs.OldItems.Count == 1)
                        {
                            return new NotifyCollectionChangedEventArgs(
                                            NotifyCollectionChangedAction.Replace,
                                            notifyCollectionChangedEventArgs.NewItems[0],
                                            notifyCollectionChangedEventArgs.OldItems[0],
                                            notifyCollectionChangedEventArgs.NewStartingIndex);
                        }
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

                    case MvxNotifyCollectionChangedAction.Move:
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

                    case MvxNotifyCollectionChangedAction.Reset:
                        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            #endregion
        }
    }
}
