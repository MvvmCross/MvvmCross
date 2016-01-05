// StyledMultilineElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace CrossUI.Droid.Dialog.Elements
{
    public class StyledMultilineElement : StringElement
    {
        public StyledMultilineElement()
        {
        }

        public StyledMultilineElement(string caption) : base(caption)
        {
        }

        public StyledMultilineElement(string caption, string value) : base(caption, value)
        {
        }

        public StyledMultilineElement(string caption, string value, string layoutName)
            : base(caption, value, layoutName)
        {
        }
    }
}