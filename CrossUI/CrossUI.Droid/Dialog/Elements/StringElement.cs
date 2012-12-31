#region Copyright

// <copyright file="StringElement.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;

namespace CrossUI.Droid.Dialog.Elements
{
    public class StringElement : StringDisplayingValueElement<string>
    {
        public object Alignment;

        public StringElement(string caption = null, string value = null, string layoutName = null)
            : base(caption, value, layoutName ?? "dialog_multiline_labelfieldbelow")
        {
            Value = value;
        }

        public override string Summary()
        {
            return Value;
        }

        public override bool Matches(string text)
        {
            return Value != null && Value.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                   base.Matches(text);
        }

        protected override string Format(string value)
        {
            return value;
        }
    }

    #region Compatibility classes

    #endregion
}