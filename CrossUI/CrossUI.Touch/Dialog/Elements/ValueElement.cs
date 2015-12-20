// ValueElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using UIKit;

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
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        protected ValueElement(string caption)
            : base(caption)
        {
            Alignment = UITextAlignment.Left;
        }

        protected ValueElement(string caption, Action tapped)
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
            set { _value = (TValueType)value; }
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

        protected ValueElement(string caption, Action tapped)
            : base(caption, tapped)
        {
        }
    }
}