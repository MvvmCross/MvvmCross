// MvxDatePicker.cs
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
using Cirrious.CrossCore.Droid.Platform;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    [Register("cirrious.mvvmcross.binding.droid.views.MvxDatePicker")]
    public class MvxDatePicker 
        : DatePicker
        , DatePicker.IOnDateChangedListener
    {
        private bool _initialized;

        public MvxDatePicker(Context context)
            : base(context)
        {
        }

        public MvxDatePicker(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

		protected MvxDatePicker(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
	    {
	    }

        public DateTime Value
        {
            get { return MvxJavaDateUtils.DateTimeFromJava(Year, Month, DayOfMonth); }
            set
            {
                var javaYear = value.Year;
                // -1 needed as Java date months are zero-based
                var javaMonth = value.Month - 1;
                var javaDay = value.Day;

                if (!_initialized)
                {
                    Init(javaYear, javaMonth, javaDay, this);
                    _initialized = true;
                } 
                else if (Year!=javaYear || Month!= javaMonth || DayOfMonth!=javaDay)
                {
                    UpdateDate(javaYear, javaMonth, javaDay);
                }
            }
        }

        public event EventHandler ValueChanged;

        public void OnDateChanged(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var handler = ValueChanged;
            if (handler != null)
            {
                handler(this, null);
            }
        }
    }
}