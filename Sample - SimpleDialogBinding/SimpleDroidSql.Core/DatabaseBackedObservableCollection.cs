using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace SimpleDroidSql
{
    public class DatabaseBackedObservableCollection<T, TKey> : IList<T>, IList, INotifyCollectionChanged where T: new()
    {
        private readonly ISQLiteConnection _connection;
        private Func<T, TKey> _sortOrder;

        public DatabaseBackedObservableCollection(ISQLiteConnection connection, Func<T, TKey> sortOrder)
        {
            _connection = connection;
            _sortOrder = sortOrder;
        }

        public void Add(T item)
        {
            _connection.Insert(item);
#warning this definitely isn't efficient!
            RaiseCollectionChanged();
        }

#warning this definitely isn't efficient!
        public int Count { get { return _connection.Table<T>().Count(); }}

        public bool Remove(T item)
        {
            var result = _connection.Delete(item);
#warning what does result mean here?
            return true;
        }

        public T this[int index]
        {
#warning this definitely isn't efficient!
            get { return _connection.Table<T>().OrderBy(_sortOrder).Skip(index).FirstOrDefault(); }
            set { throw new NotImplementedException(); }
        }


        object IList.this[int index]
        {
            get { return this[index]; }
            set { this[index] = (T)value; }
        }

        protected virtual void RaiseCollectionChanged()
        {
            var handler = CollectionChanged;
#warning this definitely isn't efficient!
            if (handler != null) handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #region This section not implemented

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize { get; private set; }

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly { get; private set; }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized { get; private set; }
        public object SyncRoot { get; private set; }

        #endregion
    }
}