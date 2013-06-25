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

        public IMvxTimeListenerTarget Target { get { return _timePicker; }}

        public EventHandler TargetValueChanged;

        public MvxTimeChangedListener(IMvxTimeListenerTarget timePicker)
        {
            _timePicker = timePicker;
        }

        public virtual void OnTimeChanged(TimePicker view, int hourOfDay, int minute)
        {
            if (TargetValueChanged!=null)
            {
                TargetValueChanged(this, EventArgs.Empty);
            }
        }
    }
}