using System;
using Android.Views;

namespace FooBar.Dialog.Droid
{
    public abstract class ValueElement : Element
    {
        protected ValueElement(string caption) : base(caption)
        {
        }

        protected ValueElement(string caption, string layoutName) : base(caption, layoutName)
        {
        }

        public event EventHandler ValueChanged;

        protected virtual void FireValueChanged()
        {
            var handler = ValueChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }

    public abstract class ValueElement<TValueType> : ValueElement
    {
        protected ValueElement(string caption, TValueType value, string layoutName)
            : base(caption, layoutName)
        {
            _value = value;
        }

        public TValueType Value
        {
            get { return _value; }
            set { _value = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        private TValueType _value;

        protected void OnUserValueChanged(TValueType newValue)
        {
            Value = newValue;
            FireValueChanged();
        }

        protected abstract void UpdateDetailDisplay(View cell);
    }
}