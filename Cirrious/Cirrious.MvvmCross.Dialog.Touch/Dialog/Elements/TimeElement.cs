using System;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public class TimeElement : DateTimeElement {
        public TimeElement (string caption, DateTime date) : base (caption, date)
        {
        }
		
        public override string FormatDate (DateTime dt)
        {
            return dt.ToLocalTime ().ToShortTimeString ();
        }
		
        public override UIDatePicker CreatePicker ()
        {
            var picker = base.CreatePicker ();
            picker.Mode = UIDatePickerMode.Time;
            return picker;
        }
    }
}