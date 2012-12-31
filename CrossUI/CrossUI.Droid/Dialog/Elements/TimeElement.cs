#region Copyright

// <copyright file="TimeElement.cs" company="Cirrious">
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
    public class TimeElement : DateTimeElement
    {
        public TimeElement(string caption, DateTime? date, string layoutName = null)
            : base(caption, date, layoutName)
        {
            Click = delegate { EditTime(); };
        }

        protected override string Format(DateTime dt)
        {
            return dt.ToShortTimeString();
        }
    }
}