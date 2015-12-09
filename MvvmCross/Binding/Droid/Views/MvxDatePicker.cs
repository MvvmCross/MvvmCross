// MvxDatePicker.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Views
{
    using System;

    using Android.Content;
    using Android.Runtime;
    using Android.Util;
    using Android.Widget;

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
            get { return MvxJavaDateUtils.DateTimeFromJava(this.Year, this.Month, this.DayOfMonth); }
            set
            {
                var javaYear = value.Year;
                // -1 needed as Java date months are zero-based
                var javaMonth = value.Month - 1;
                var javaDay = value.Day;

                if (!this._initialized)
                {
                    this.Init(javaYear, javaMonth, javaDay, this);
                    this._initialized = true;
                }
                else if (this.Year != javaYear || this.Month != javaMonth || this.DayOfMonth != javaDay)
                {
                    this.UpdateDate(javaYear, javaMonth, javaDay);
                }
            }
        }

        public event EventHandler ValueChanged;

        public void OnDateChanged(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var handler = this.ValueChanged;
            handler?.Invoke(this, null);
        }
    }
}