// NotifyChange.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace MvvmCross.FieldBinding
{
    public class NotifyChange
        : MvxMainThreadDispatchingObject
          , INotifyChange
    {
        private bool _shouldAlwaysRaiseChangedOnUserInterfaceThread;

        public NotifyChange()
        {
            _shouldAlwaysRaiseChangedOnUserInterfaceThread = true;
        }

        protected NotifyChange(object value)
        {
            _value = value;
        }

        protected NotifyChange(object value, Action<object> valueChanged)
        {
            _value = value;
            Changed += (s, e) => { valueChanged?.Invoke(Value); };
        }

        public bool ShouldAlwaysRaiseChangedOnUserInterfaceThread()
        {
            return _shouldAlwaysRaiseChangedOnUserInterfaceThread;
        }

        public void ShouldAlwaysRaiseChangedOnUserInterfaceThread(bool value)
        {
            _shouldAlwaysRaiseChangedOnUserInterfaceThread = value;
        }

        public event EventHandler Changed;

        public void RaiseChanged()
        {
            var raiseAction = new Action(() =>
                {
                    var handler = Changed;

                    handler?.Invoke(this, EventArgs.Empty);
                });

            if (ShouldAlwaysRaiseChangedOnUserInterfaceThread())
            {
                if (Changed == null)
                    return;
                InvokeOnMainThread(raiseAction);
            }
            else
            {
                raiseAction();
            }
        }

        private object _value;

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaiseChanged();
            }
        }

        public Type ValueType { get; protected set; }
    }

    public class NotifyChange<T>
        : NotifyChange
          , INotifyChange<T>
    {
        public new T Value
        {
            get { return (T)base.Value; }
            set { base.Value = value; }
        }

        public NotifyChange()
            : this(default(T))
        {
        }

        public NotifyChange(T value)
            : base(value)
        {
            ValueType = typeof(T);
        }

        public NotifyChange(T value, Action<T> valueChanged)
            : base(value, obj => valueChanged?.Invoke((T)obj))
        {
            ValueType = typeof(T);
        }
    }

    public class NotifyChangeList<TValue>
        : NotifyChange<IList<TValue>>
        , INotifyChangeList<TValue>
    {
        public NotifyChangeList()
            : base()
        {
        }

        public NotifyChangeList(IList<TValue> value)
            : base(value)
        {
        }

        public NotifyChangeList(IList<TValue> value, Action<IList<TValue>> valueChanged)
            : base(value, valueChanged)
        {
        }

        // this indexer will never actually be used in binding
        // is used to assist the Fluent syntax in https://github.com/slodge/MvvmCross/issues/353
        // but the underlying binding will use the indexer on the collection, not on this NotifyChange object
        public TValue this[int key]
        {
            get { return Value[key]; }
            set { Value[key] = value; }
        }
    }

    public class NotifyChangeDictionary<TKey, TValue>
        : NotifyChange<IDictionary<TKey, TValue>>
        , INotifyChangeDictionary<TKey, TValue>
    {
        public NotifyChangeDictionary()
            : base()
        {
        }

        public NotifyChangeDictionary(IDictionary<TKey, TValue> value)
            : base(value)
        {
        }

        public NotifyChangeDictionary(IDictionary<TKey, TValue> value, Action<IDictionary<TKey, TValue>> valueChanged)
            : base(value, valueChanged)
        {
        }

        // this indexer will never actually be used in binding
        // is used to assist the Fluent syntax in https://github.com/slodge/MvvmCross/issues/353
        // but the underlying binding will use the indexer on the collection, not on this NotifyChange object
        public TValue this[TKey key]
        {
            get { return Value[key]; }
            set { Value[key] = value; }
        }
    }
}