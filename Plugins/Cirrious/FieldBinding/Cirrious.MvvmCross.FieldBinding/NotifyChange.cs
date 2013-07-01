// NotifyChange.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.FieldBinding
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
            Changed += (s, e) => { valueChanged(Value); };
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

                    if (handler != null)
                        handler(this, EventArgs.Empty);
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
            get { return (T) base.Value; }
            set { base.Value = value; }
        }

        public NotifyChange()
            : this(default(T))
        {
        }

        public NotifyChange(T value)
            : base(value)
        {
            ValueType = typeof (T);
        }

        public NotifyChange(T value, Action<T> valueChanged)
            : base(value, obj => valueChanged((T) obj))
        {
            ValueType = typeof (T);
        }
    }
}