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
using MonoTouch.UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class TimeElement : DateTimeElement
    {
        public TimeElement()
            : base("", DateTime.Now)
        {
        }

        public TimeElement(string caption, DateTime date)
            : base(caption, date)
        {
        }

        public override string FormatDate(DateTime dt)
        {
            return dt.ToLocalTime().ToShortTimeString();
        }

        public override UIDatePicker CreatePicker()
        {
            var picker = base.CreatePicker();
            picker.Mode = UIDatePickerMode.Time;
            return picker;
        }
    }
}