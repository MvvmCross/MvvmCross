// MvxTimeChangedListener.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxTimeChangedListener
        : Java.Lang.Object
          , TimePicker.IOnTimeChangedListener
    {
        private readonly IMvxTimeListenerTarget _timePicker;

        public MvxTimeChangedListener(IMvxTimeListenerTarget timePicker)
        {
            _timePicker = timePicker;
        }

        public void OnTimeChanged(TimePicker view, int hourOfDay, int minute)
        {
            _timePicker.InternalSetValueAndRaiseChanged(new TimeSpan(hourOfDay, minute, 0));
        }
    }
}