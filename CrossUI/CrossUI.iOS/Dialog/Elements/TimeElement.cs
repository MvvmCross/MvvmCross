// TimeElement.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using UIKit;

namespace CrossUI.iOS.Dialog.Elements
{
    public class TimeElement : DateTimeElement
    {
        public TimeElement()
            : base("", null)
        {
        }

        public TimeElement(string caption, DateTime? date)
            : base(caption, date)
        {
            DateTimeFormat = "t";
        }

        public override UIDatePicker CreatePicker()
        {
            var picker = base.CreatePicker();
            picker.Mode = UIDatePickerMode.Time;
            return picker;
        }
    }
}