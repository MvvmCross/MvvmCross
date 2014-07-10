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
    public class MvxAdapterViewSelectedItemPositionTargetBinding 
        : MvxAndroidTargetBinding
    {
        protected AdapterView AdapterView
        {
            get { return (AdapterView) Target; }
        }

        private bool _subscribed;

        public MvxAdapterViewSelectedItemPositionTargetBinding(AdapterView adapterView)
            : base(adapterView)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            ((AdapterView)target).SetSelection((int) value);
        }

        private void AdapterViewOnItemSelected(object sender, AdapterView.ItemSelectedEventArgs itemSelectedEventArgs)
        {
            FireValueChanged(itemSelectedEventArgs.Position);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override void SubscribeToEvents()
        {
            var adapterView = AdapterView;

            if (adapterView == null)
                return;

            _subscribed = true;
            adapterView.ItemSelected += AdapterViewOnItemSelected;
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
                if (adapterView != null && _subscribed)
                {
                    adapterView.ItemSelected -= AdapterViewOnItemSelected;
                    _subscribed = false;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}