// DateElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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