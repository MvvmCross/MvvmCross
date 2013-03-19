using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrious.MvvmCross.Plugins.SystemData.Wpf
{
    public class SQLiteParameterCollection : Cirrious.MvvmCross.Plugins.SystemData.IDataParameterCollection
    {
        private System.Data.IDataParameterCollection collection;
        private System.Data.IDbCommand command;


        internal SQLiteParameterCollection(System.Data.IDataParameterCollection dataParameterCollection, System.Data.IDbCommand dbCommand)
        {
            this.collection = dataParameterCollection;
            this.command = dbCommand;
        }

        public object this[string parameterName]
        {
            get
            {
                return collection[parameterName];
            }
            set
            {
                collection[parameterName] = value;
            }
        }

        public bool Contains(string parameterName)
        {
            return collection.Contains(parameterName);
        }

        public int IndexOf(string parameterName)
        {
            return collection.IndexOf(parameterName);
        }

        public void RemoveAt(string parameterName)
        {
            collection.RemoveAt(parameterName);
        }

        public int Add(object value)
        {
            return collection.Add(value);
        }

        public int Add(string parameterName, object value)
        {
            var parameter = this.command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            return this.Add(parameter);
        }

        public void Clear()
        {
            collection.Clear();
        }

        public bool Contains(object value)
        {
            return collection.Contains(value);
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            collection.Insert(index, value);
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            collection.Remove(value);
        }

        public void RemoveAt(int index)
        {
            collection.RemoveAt(index);
        }

        public object this[int index]
        {
            get
            {
                return collection[index];
            }
            set
            {
                collection[index] = value;
            }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return collection.Count; }
        }

        public bool IsSynchronized
        {
            get { return collection.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return collection.SyncRoot; }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return collection.GetEnumerator();
        }
    }
}
