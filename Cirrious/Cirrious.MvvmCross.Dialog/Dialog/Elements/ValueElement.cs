using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    /// <summary>
    ///   The string element can be used to render some text in a cell 
    ///   that can optionally respond to tap events.
    /// </summary>
    public abstract class ValueElement<TValueType> : Element 
    {
        private UITextAlignment _alignment;
        public UITextAlignment Alignment
        {
            get { return _alignment; }
            set { _alignment = value; ActOnCurrentAttachedCell(UpdateCaptionDisplay); }
        }

        private TValueType _value;
        public TValueType Value
        {
            get { return _value; }
            set { _value = value; ActOnCurrentAttachedCell(UpdateDetailDisplay); }
        }

        public event EventHandler ValueChanged;
        protected void OnUserValueChanged(TValueType newValue)
        {
            Value = newValue;
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }

        protected ValueElement (string caption) : base (caption) {}

        protected ValueElement(string caption, TValueType value)
            : base(caption)
        {
            Value = value;
            Alignment = UITextAlignment.Left;
        }

        protected ValueElement (string caption,  NSAction tapped) 
            : base (caption, tapped)
        {
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

        public override string Summary ()
        {
            return Caption;
        }		
    }
}