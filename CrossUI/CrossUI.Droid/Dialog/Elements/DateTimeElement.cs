using System;
using Android.App;
using Android.Text.Format;

namespace CrossUI.Droid.Dialog.Elements
{
    public class DateTimeElement : StringDisplayingValueElement<DateTime>
    {
        public int MinuteInterval { get; set; }

        public DateTimeElement(string caption, DateTime? date, string layoutName = null)
            : base(caption, date ?? DateTime.Now, layoutName ?? "dialog_multiline_labelfieldbelow")
        {
            Click = delegate { EditDate(); };
        }

        protected override string Format(DateTime dt)
        {
            return dt.ToShortDateString() + " " + dt.ToShortTimeString();
        }

        protected void EditDate()
        {
            var context = Context;
            if (context == null)
            {
                Android.Util.Log.Warn("DateElement", "No Context for Edit");
                return;
            }
            var val = Value;
            new DatePickerDialog(context, DateCallback ?? OnDateTimeSet, val.Year, val.Month - 1, val.Day).Show();
        }

        protected void EditTime()
        {
            var context = Context;
            if (context == null)
            {
                Android.Util.Log.Warn("TimeElement", "No Context for Edit");
                return;
            }
            DateTime val = Value;
            new TimePickerDialog(context, OnTimeSet, val.Hour, val.Minute, DateFormat.Is24HourFormat(context)).Show();
        }

        // the event received when the user "sets" the date in the dialog
        protected void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            DateTime current = Value;
            OnUserValueChanged(new DateTime(e.Date.Year, e.Date.Month, e.Date.Day, current.Hour, current.Minute, 0));
        }

        // the event received when the user "sets" the date in the dialog
        protected void OnDateTimeSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            DateTime current = Value;
            OnUserValueChanged(new DateTime(e.Date.Year, e.Date.Month, e.Date.Day, current.Hour, current.Minute, 0));
            EditTime();
        }

        // the event received when the user "sets" the time in the dialog
        protected void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            DateTime current = Value;
            OnUserValueChanged(new DateTime(current.Year, current.Month, current.Day, e.HourOfDay, e.Minute, 0));
        }

        protected EventHandler<DatePickerDialog.DateSetEventArgs> DateCallback = null;
    }
}