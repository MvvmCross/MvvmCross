// MvxDateChangedListener.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Widget;
using Cirrious.CrossCore.Droid.Platform;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxDateChangedListener
        : Java.Lang.Object
          , DatePicker.IOnDateChangedListener
    {
        private readonly IMvxDateListenerTarget _target;

        public MvxDateChangedListener(IMvxDateListenerTarget target)
        {
            _target = target;
        }

        public void OnDateChanged(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var dateTime = MvxJavaDateUtils.DateTimeFromJava(year, monthOfYear, dayOfMonth);
            _target.InternalSetValueAndRaiseChanged(dateTime);
        }
    }
}