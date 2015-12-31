// DateElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using UIKit;

namespace CrossUI.iOS.Dialog.Elements
{
    public class DateElement : DateTimeElement
    {
        public DateElement(string caption = null) : this(caption, DateTime.Now)
        {
        }

        public DateElement(string caption, DateTime date) : base(caption, date)
        {
            DateTimeFormat = "MMM d, yyyy";
        }

        public override UIDatePicker CreatePicker()
        {
            var picker = base.CreatePicker();
            picker.Mode = UIDatePickerMode.Date;
            return picker;
        }
    }
}