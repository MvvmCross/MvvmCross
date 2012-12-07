using System;

namespace CrossUI.Droid.Dialog.Elements
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