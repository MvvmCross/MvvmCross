// BoolElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace CrossUI.Droid.Dialog.Elements
{
    public abstract class BoolElement : ValueElement<bool>
    {
        public string TextOff { get; set; }
        public string TextOn { get; set; }

        protected BoolElement(string caption = null, bool value = false, string layoutName = null)
            : base(caption, value, layoutName)
        {
        }

        public override string Summary()
        {
            return Value ? TextOn : TextOff;
        }
    }
}