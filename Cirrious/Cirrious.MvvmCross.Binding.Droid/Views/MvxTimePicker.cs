// MvxTimePicker.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Util;
using Android.Widget;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    // Special thanks for this file to Emi - https://github.com/eMi-/mvvmcross_datepicker_timepicker
    // Code used under Creative Commons with attribution
    // See also http://stackoverflow.com/questions/14829521/bind-timepicker-datepicker-mvvmcross-mono-for-android
    public class MvxTimePicker
        : TimePicker
          , IMvxTimeListenerTarget
    {
        public MvxTimePicker(Context context)
            : base(context)
        {
            SetOnTimeChangedListener(new MvxTimeChangedListener(this));
        }

        public MvxTimePicker(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            SetOnTimeChangedListener(new MvxTimeChangedListener(this));
        }

        public override sealed void SetOnTimeChangedListener(IOnTimeChangedListener onTimeChangedListener)
        {
            base.SetOnTimeChangedListener(onTimeChangedListener);
        }

        public TimeSpan Value
        {
            get
            {
                int currentHour = CurrentHour.IntValue();
                int currentMinute = CurrentMinute.IntValue();
                return new TimeSpan(currentHour, currentMinute, 0);
            }
            set
            {
                CurrentHour = new Java.Lang.Integer(value.Hours);
                CurrentMinute = new Java.Lang.Integer(value.Minutes);
            }
        }

        public event EventHandler ValueChanged;

        public void InternalSetValueAndRaiseChanged(TimeSpan timeSpan)
        {
            Value = timeSpan;
            EventHandler handler = ValueChanged;
            if (handler != null)
            {
                handler(this, null);
            }
        }
    }
}