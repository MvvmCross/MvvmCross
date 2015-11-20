// RadioGroup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace CrossUI.Droid.Dialog.Elements
{
    /// <summary>
    /// Captures the information about mutually exclusive elements in a RootElement
    /// </summary>
    public class RadioGroup : Group
    {
        public int Selected { get; set; }

        public RadioGroup()
        {
        }

        public RadioGroup(string key, int selected)
            : base(key)
        {
            Selected = selected;
        }

        public RadioGroup(int selected)
            : base(null)
        {
            Selected = selected;
        }
    }
}