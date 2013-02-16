using System;
using Android.Widget;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxTimeChangedListener
        : Java.Lang.Object
          , TimePicker.IOnTimeChangedListener
    {
        private IMvxTimeListenerTarget _timePicker;

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