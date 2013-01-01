// MvxAdapterViewSelectedItemPositionTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxAdapterViewSelectedItemPositionTargetBinding : MvxBaseAndroidTargetBinding
    {
        private readonly AdapterView _adapterView;

        public MvxAdapterViewSelectedItemPositionTargetBinding(AdapterView adapterView)
        {
            _adapterView = adapterView;
            _adapterView.ItemSelected += AdapterViewOnItemSelected;
        }

        public override void SetValue(object value)
        {
            _adapterView.SetSelection((int) value);
        }

        private void AdapterViewOnItemSelected(object sender, AdapterView.ItemSelectedEventArgs itemSelectedEventArgs)
        {
            FireValueChanged(itemSelectedEventArgs.Position);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override Type TargetType
        {
            get { return typeof (Int32); }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_adapterView != null)
                {
                    _adapterView.ItemSelected -= AdapterViewOnItemSelected;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}