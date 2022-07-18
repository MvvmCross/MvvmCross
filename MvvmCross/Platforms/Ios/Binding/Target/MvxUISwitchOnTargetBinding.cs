// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Target
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
                MvxBindingLog.Error("Error - Switch is null in MvxUISwitchOnTargetBinding");
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
