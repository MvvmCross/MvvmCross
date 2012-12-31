#region Copyright

// <copyright file="ValueElement.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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
            if (handler != null)
                handler(this, EventArgs.Empty);
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
            Value = newValue;
            FireValueChanged();
        }
    }
}