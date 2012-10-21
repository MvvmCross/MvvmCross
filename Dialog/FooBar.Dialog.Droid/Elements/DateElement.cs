using System;

namespace FooBar.Dialog.Droid
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