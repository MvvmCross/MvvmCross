#region Copyright

// <copyright file="StringMultilineElement.cs" company="Cirrious">
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