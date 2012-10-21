using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace FooBar.Dialog.Droid
{
    public class StringElement : StringDisplayingValueElement<string>
    {
        public object Alignment;

        public StringElement(string caption = null, string value = null, string layoutName = null)
            : base(caption, value, layoutName ?? "dialog_multiline_labelfieldbelow")
        {
            Value = value;
        }

        public override string Summary()
        {
            return Value;
        }

        public override bool Matches(string text)
        {
            return Value != null && Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 || base.Matches(text);
        }

        protected override string Format(string value)
        {
            return value;
        }
    }

    #region Compatibility classes
    public class MultilineElement : StringElement
    {
        public MultilineElement(string caption) : base(caption) { }
        public MultilineElement(string caption, string value) : base(caption, value) { }
        public MultilineElement(string caption, string value, string layoutName) : base(caption, value, layoutName) { }
    }

    #endregion
}