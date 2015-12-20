// StringElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace CrossUI.Droid.Dialog.Elements
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
            return Value != null && Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                   base.Matches(text);
        }

        protected override string Format(string value)
        {
            return value;
        }
    }
}