#region Copyright

// <copyright file="RadioGroup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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