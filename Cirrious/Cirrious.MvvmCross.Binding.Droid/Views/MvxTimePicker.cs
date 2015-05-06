// MvxTimePicker.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    // Special thanks for this file to Emi - https://github.com/eMi-/mvvmcross_datepicker_timepicker
    // Code used under Creative Commons with attribution
    // See also http://stackoverflow.com/questions/14829521/bind-timepicker-datepicker-mvvmcross-mono-for-android
    [Register("cirrious.mvvmcross.binding.droid.views.MvxTimePicker")]
    public class MvxTimePicker
        : TimePicker
        , TimePicker.IOnTimeChangedListener
    {
        private bool _initialized;

        public MvxTimePicker(Context context)
            : base(context)
        {
        }

        public MvxTimePicker(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

		protected MvxTimePicker(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
	    {
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
                var javaHour = new Java.Lang.Integer(value.Hours);
                var javaMinutes =new Java.Lang.Integer(value.Minutes);

                if (!_initialized)
                {
                    SetOnTimeChangedListener(this);
                    _initialized = true;
                }

                if (CurrentHour != javaHour)
                {
                    CurrentHour = javaHour;
                }
                if (CurrentMinute != javaMinutes)
                {
                    CurrentMinute = javaMinutes;
                }
            }
        }

        public event EventHandler ValueChanged;

        public void OnTimeChanged(TimePicker view, int hourOfDay, int minute)
        {
            EventHandler handler = ValueChanged;
            if (handler != null)
            {
                handler(this, null);
            }
        }
    }
}