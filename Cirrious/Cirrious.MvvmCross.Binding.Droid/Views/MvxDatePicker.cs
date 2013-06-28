// MvxDatePicker.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Util;
using Android.Widget;
using Cirrious.CrossCore.Droid.Platform;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxDatePicker : DatePicker, IMvxDateListenerTarget
    {
        public MvxDatePicker(Context context)
            : base(context)
        {
        }

        public MvxDatePicker(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        private bool _initialised;

        public override void Init(int year, int monthOfYear, int dayOfMonth, DatePicker.IOnDateChangedListener onDateChangedListener)
        {
            base.Init(year, monthOfYear, dayOfMonth, onDateChangedListener);
            _initialised = true;
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

                if (!_initialised)
                {
                    Init(javaYear, javaMonth, javaDay, new MvxDateChangedListener(this));
                } 
                else  if (Year!=javaYear || Month!= javaMonth || DayOfMonth!=javaDay)
                {
                    UpdateDate(javaYear, javaMonth, javaDay);

                    var handler = ValueChanged;
                    if (handler != null)
                    {
                        handler(this, null);
                    }
                }
            }
        }

        public event EventHandler ValueChanged;

    }
}