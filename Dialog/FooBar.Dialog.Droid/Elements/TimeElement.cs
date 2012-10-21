using System;

namespace FooBar.Dialog.Droid
{
    public class TimeElement : DateTimeElement
    {
        public TimeElement(string caption, DateTime? date, string layoutName = null)
            : base(caption, date, layoutName)
        {
            Click = delegate { EditTime(); };
        }

        protected override string Format(DateTime dt)
        {
            return dt.ToShortTimeString();
        }
    }
}