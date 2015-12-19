// RadioGroup.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace CrossUI.Touch.Dialog.Elements
{
    /// <summary>
    /// Captures the information about mutually exclusive elements in a RootElement
    /// </summary>
    public class RadioGroup : Group
    {
        private int selected;

        public virtual int Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public RadioGroup(string key = null, int selected = 0) : base(key)
        {
            this.selected = selected;
        }

        public RadioGroup(int selected) : base(null)
        {
            this.selected = selected;
        }
    }
}