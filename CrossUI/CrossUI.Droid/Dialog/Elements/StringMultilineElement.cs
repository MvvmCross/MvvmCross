// StringMultilineElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace CrossUI.Droid.Dialog.Elements
{
    public class StringMultilineElement : StringElement
    {
        public StringMultilineElement()
        {
        }

        public StringMultilineElement(string caption) : base(caption)
        {
        }

        public StringMultilineElement(string caption, string value) : base(caption, value)
        {
        }

        public StringMultilineElement(string caption, string value, string layoutName)
            : base(caption, value, layoutName)
        {
        }
    }
}