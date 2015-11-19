// ValueElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;

namespace CrossUI.Droid.Dialog.Elements
{
    public abstract class ValueElement : Element
    {
        protected ValueElement(string caption) : base(caption)
        {
        }

        protected ValueElement(string caption, object value, string layoutName) : base(caption, layoutName)
        {
            _value = value;
        }

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
        }

        private object _value;

        public event EventHandler ValueChanged;

        protected virtual void FireValueChanged()
        {
            var handler = ValueChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected abstract void UpdateDetailDisplay(View cell);
    }

    public abstract class ValueElement<TValueType> : ValueElement
    {
        protected ValueElement(string caption, TValueType value, string layoutName)
            : base(caption, value, layoutName)
        {
        }

        public new TValueType Value
        {
            get { return (TValueType) base.Value; }
            set { base.Value = value; }
        }

        protected void OnUserValueChanged(TValueType newValue)
        {
            if (MatchesExistingValue(newValue)) 
                return;

            Value = newValue;
            FireValueChanged();
        }

        protected bool MatchesExistingValue(TValueType newValue)
        {
            if (typeof (TValueType).IsValueType)
            {
                if (Value.Equals(newValue))
                    return true;

                return false;
            }

            if (Value == null)
            {
                if (newValue == null)
                    return true;

                return false;
            }

            return Value.Equals(newValue);
        }
    }
}