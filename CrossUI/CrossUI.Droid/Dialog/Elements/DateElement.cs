#region Copyright

// <copyright file="DateElement.cs" company="Cirrious">
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
    public class DateElement : DateTimeElement
    {
        public DateElement(string caption, DateTime? date, string layoutName = null)
            : base(caption, date, layoutName)
        {
            DateCallback = OnDateSet;
        }

        protected override string Format(DateTime dt)
        {
            return dt.ToShortDateString();
        }
    }
}