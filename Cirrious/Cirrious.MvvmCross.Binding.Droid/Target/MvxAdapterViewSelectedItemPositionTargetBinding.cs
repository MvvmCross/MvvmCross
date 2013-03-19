// MvxAdapterViewSelectedItemPositionTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxAdapterViewSelectedItemPositionTargetBinding : MvxAndroidTargetBinding
    {
        protected AdapterView AdapterView
        {
            get { return (AdapterView) Target; }
        }

        public MvxAdapterViewSelectedItemPositionTargetBinding(AdapterView adapterView)
            : base(adapterView)
        {
            adapterView.ItemSelected += AdapterViewOnItemSelected;
        }

        public override void SetValue(object value)
        {
            var adapterView = AdapterView;
            if (adapterView == null)
                return;
            adapterView.SetSelection((int) value);
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
                var adapterView = AdapterView;
                if (adapterView != null)
                {
                    adapterView.ItemSelected -= AdapterViewOnItemSelected;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}