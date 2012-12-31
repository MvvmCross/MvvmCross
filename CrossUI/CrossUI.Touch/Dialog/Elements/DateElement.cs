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
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class DateElement : DateTimeElement
    {
        public DateElement(string caption, DateTime date) : base(caption, date)
        {
            Formatter.DateStyle = NSDateFormatterStyle.Medium;
        }

        public override string FormatDate(DateTime dt)
        {
            return Formatter.ToString(dt);
        }

        public override UIDatePicker CreatePicker()
        {
            var picker = base.CreatePicker();
            picker.Mode = UIDatePickerMode.Date;
            return picker;
        }
    }
}