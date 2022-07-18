// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    // Special thanks for this file to Emi - https://github.com/eMi-/mvvmcross_datepicker_timepicker
    // Code used under Creative Commons with attribution
    // See also http://stackoverflow.com/questions/14829521/bind-timepicker-datepicker-mvvmcross-mono-for-android
    [Register("mvvmcross.platforms.android.binding.views.MvxTimePicker")]
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

        public MvxTimePicker(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public MvxTimePicker(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
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
                return new TimeSpan(Hour, Minute, 0);
            }
            set
            {
                if (!_initialized)
                {
                    SetOnTimeChangedListener(this);
                    _initialized = true;
                }

                if (Hour != value.Hours)
                {
                    Hour = value.Hours;
                }
                if (Minute != value.Minutes)
                {
                    Minute = value.Minutes;
                }
            }
        }

        public event EventHandler ValueChanged;

        public void OnTimeChanged(TimePicker view, int hourOfDay, int minute)
        {
            ValueChanged?.Invoke(this, null);
        }
    }
}
