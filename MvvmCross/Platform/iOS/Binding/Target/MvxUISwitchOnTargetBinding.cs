// MvxUISwitchOnTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUISwitchOnTargetBinding : MvxTargetBinding<UISwitch, bool>
    {
        private IDisposable _subscription;

        public MvxUISwitchOnTargetBinding(UISwitch target)
            : base(target)
        {
        }

        protected override void SetValue(bool value)
        {
            Target.SetState(value, true);
        }

        public override void SubscribeToEvents()
        {
            var uiSwitch = Target;
            if (uiSwitch == null)
            {
                MvxBindingTrace.Error( "Error - Switch is null in MvxUISwitchOnTargetBinding");
                return;
            }

            _subscription = uiSwitch.WeakSubscribe(nameof(uiSwitch.ValueChanged), HandleValueChanged);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }

        private void HandleValueChanged(object sender, EventArgs e)
        {
            FireValueChanged(Target.On);
        }
    }
}