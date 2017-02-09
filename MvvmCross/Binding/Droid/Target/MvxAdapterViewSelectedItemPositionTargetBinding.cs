// MvxAdapterViewSelectedItemPositionTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxAdapterViewSelectedItemPositionTargetBinding
        : MvxAndroidTargetBinding
    {
        protected AdapterView AdapterView => (AdapterView)Target;

        private IDisposable _subscription;

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
            var adapterView = AdapterView;

            if (adapterView == null)
                return;

            _subscription = adapterView.WeakSubscribe<AdapterView, AdapterView.ItemSelectedEventArgs>(
                nameof(adapterView.ItemSelected), AdapterViewOnItemSelected);
        }

        public override Type TargetType => typeof(Int32);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }

            base.Dispose(isDisposing);
        }
    }
}