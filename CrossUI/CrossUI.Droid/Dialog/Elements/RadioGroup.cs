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