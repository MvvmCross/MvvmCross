﻿// DateTimeElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.Text.Format;
using CrossUI.Core;

namespace CrossUI.Droid.Dialog.Elements
{
    public class DateTimeElement : StringDisplayingValueElement<DateTime>
    {
        public int MinuteInterval { get; set; }

        public DateTimeElement(string caption = null, DateTime? date = null, string layoutName = null)
            : this(caption, date ?? DateTime.UtcNow, layoutName)
        {
        }

        public DateTimeElement(string caption, DateTime date, string layoutName = null)
            : base(caption, date, layoutName ?? "dialog_multiline_labelfieldbelow")
        {
            Click = delegate { EditDate(); };
            DateTimeFormat = "G";
            if (Value.Kind != DateTimeKind.Utc)
                DialogTrace.WriteLine("Warning - non Utc datetmie used within DateTimeElement - can lead to unpredictable results");
        }

        protected override string Format(DateTime dt)
        {
            return dt.ToString(DateTimeFormat);
        }

        public string DateTimeFormat { get; set; }

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
            OnUserValueChanged(new DateTime(e.Date.Year, e.Date.Month, e.Date.Day, current.Hour, current.Minute, 0, DateTimeKind.Utc));
        }

        // the event received when the user "sets" the date in the dialog
        protected void OnDateTimeSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            DateTime current = Value;
            OnUserValueChanged(new DateTime(e.Date.Year, e.Date.Month, e.Date.Day, current.Hour, current.Minute, 0, DateTimeKind.Utc));
            EditTime();
        }

        // the event received when the user "sets" the time in the dialog
        protected void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            DateTime current = Value;
            OnUserValueChanged(new DateTime(current.Year, current.Month, current.Day, e.HourOfDay, e.Minute, 0, DateTimeKind.Utc));
        }

        protected EventHandler<DatePickerDialog.DateSetEventArgs> DateCallback = null;
    }
}