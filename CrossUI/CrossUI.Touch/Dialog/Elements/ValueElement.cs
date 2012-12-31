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
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public abstract class ValueElement : Element
    {
        private UITextAlignment _alignment;

        public UITextAlignment Alignment
        {
            get { return _alignment; }
            set
            {
                _alignment = value;
                ActOnCurrentAttachedCell(UpdateCaptionDisplay);
            }
        }

        public abstract object ObjectValue { get; set; }

        public event EventHandler ValueChanged;

        protected void FireValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }

        protected ValueElement(string caption)
            : base(caption)
        {
            Alignment = UITextAlignment.Left;
        }

        protected ValueElement(string caption, NSAction tapped)
            : base(caption, tapped)
        {
            Alignment = UITextAlignment.Left;
        }

        protected override void UpdateCellDisplay(UITableViewCell cell)
        {
            UpdateDetailDisplay(cell);
            base.UpdateCellDisplay(cell);
        }

        protected abstract void UpdateDetailDisplay(UITableViewCell cell);

        protected override void UpdateCaptionDisplay(UITableViewCell cell)
        {
            if (cell == null)
                return;

            cell.TextLabel.Text = Caption;
            cell.TextLabel.TextAlignment = Alignment;
        }

        public override string Summary()
        {
            return Caption;
        }
    }

    public abstract class ValueElement<TValueType> : ValueElement
    {
        private TValueType _value;

        public TValueType Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ActOnCurrentAttachedCell(UpdateDetailDisplay);
            }
        }

        public override object ObjectValue
        {
            get { return _value; }
            set { _value = (TValueType) value; }
        }

        protected void OnUserValueChanged(TValueType newValue)
        {
            Value = newValue;
            FireValueChanged();
        }

        protected ValueElement(string caption)
            : base(caption)
        {
        }

        protected ValueElement(string caption, TValueType value)
            : base(caption)
        {
            Value = value;
        }

        protected ValueElement(string caption, NSAction tapped)
            : base(caption, tapped)
        {
        }
    }
}