using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public class DateElement : DateTimeElement 
    {
        public DateElement (string caption, DateTime date) : base (caption, date)
        {
            Formatter.DateStyle = NSDateFormatterStyle.Medium;
        }
		
        public override string FormatDate (DateTime dt)
        {
            return Formatter.ToString (dt);
        }
		
        public override UIDatePicker CreatePicker ()
        {
            var picker = base.CreatePicker ();
            picker.Mode = UIDatePickerMode.Date;
            return picker;
        }
    }
}