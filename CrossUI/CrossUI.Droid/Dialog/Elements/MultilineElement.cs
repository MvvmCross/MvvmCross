// MultilineElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace CrossUI.Droid.Dialog.Elements
{
    public class MultilineElement : StringElement
    {
        public MultilineElement(string caption = null) : base(caption)
        {
        }

        public MultilineElement(string caption, string value) : base(caption, value)
        {
        }

        public MultilineElement(string caption, string value, string layoutName) : base(caption, value, layoutName)
        {
        }
    }
}