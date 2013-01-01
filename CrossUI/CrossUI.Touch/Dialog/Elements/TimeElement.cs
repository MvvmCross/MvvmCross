// TimeElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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