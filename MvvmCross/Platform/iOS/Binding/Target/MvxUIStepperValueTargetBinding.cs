// MvxUIStepperValueTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt

using System;
using System.Reflection;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUIStepperValueTargetBinding : MvxPropertyInfoTargetBinding<UIStepper>
    {
        private IDisposable _subscription;

        public MvxUIStepperValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as UIStepper;
            if (view == null) return;

            view.Value = (double)value;
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.Value);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var stepper = View;
            if (stepper == null)
            {
                MvxBindingTrace.Error( "UIStepper is null in MvxUIStepperValueTargetBinding");
                return;
            }

            _subscription = stepper.WeakSubscribe(nameof(stepper.ValueChanged), HandleValueChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}