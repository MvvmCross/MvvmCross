// MvxAdapterViewSelectedItemPositionTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxAdapterViewSelectedItemPositionTargetBinding
        : MvxAndroidTargetBinding
    {
        protected AdapterView AdapterView => (AdapterView)Target;

        private bool _subscribed;

        public MvxAdapterViewSelectedItemPositionTargetBinding(AdapterView adapterView)
            : base(adapterView)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            ((AdapterView)target).SetSelection((int)value);
        }

        private void AdapterViewOnItemSelected(object sender, AdapterView.ItemSelectedEventArgs itemSelectedEventArgs)
        {
            FireValueChanged(itemSelectedEventArgs.Position);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var adapterView = this.AdapterView;

            if (adapterView == null)
                return;

            this._subscribed = true;
            adapterView.ItemSelected += this.AdapterViewOnItemSelected;
        }

        public override Type TargetType => typeof(Int32);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var adapterView = this.AdapterView;
                if (adapterView != null && this._subscribed)
                {
                    adapterView.ItemSelected -= this.AdapterViewOnItemSelected;
                    this._subscribed = false;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}