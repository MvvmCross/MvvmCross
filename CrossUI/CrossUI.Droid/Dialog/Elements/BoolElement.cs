#region Copyright

// <copyright file="BoolElement.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

namespace CrossUI.Droid.Dialog.Elements
{
    public abstract class BoolElement : ValueElement<bool>
    {
        public string TextOff { get; set; }
        public string TextOn { get; set; }

        protected BoolElement(string caption, bool value, string layoutName = null)
            : base(caption, value, layoutName)
        {
        }

        public override string Summary()
        {
            return Value ? TextOn : TextOff;
        }
    }
}