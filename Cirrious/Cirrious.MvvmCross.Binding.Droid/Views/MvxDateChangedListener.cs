// MvxDateChangedListener.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using Cirrious.CrossCore.Droid.Platform;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public class MvxDateChangedListener
        : Java.Lang.Object
          , DatePicker.IOnDateChangedListener
    {
        private readonly IMvxDateListenerTarget _target;

        public event EventHandler TargetValueChanged;

        public MvxDateChangedListener(IMvxDateListenerTarget target)
        {
            _target = target;
        }

        public IMvxDateListenerTarget Target { get { return _target; }}

        public virtual void OnDateChanged(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var handler = TargetValueChanged;
            if (handler!=null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}