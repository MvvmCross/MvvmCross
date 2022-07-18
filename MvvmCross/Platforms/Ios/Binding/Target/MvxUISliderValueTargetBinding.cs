// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Target
{
    public class MvxUISliderValueTargetBinding : MvxPropertyInfoTargetBinding<UISlider>
    {
        private IDisposable _subscription;

        public MvxUISliderValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as UISlider;
            if (view == null) return;

            view.Value = (float)value;
        }

        private void HandleSliderValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.Value);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var slider = View;
            if (slider == null)
            {
                MvxBindingLog.Error("Error - UISlider is null in MvxUISliderValueTargetBinding");
                return;
            }

            _subscription = slider.WeakSubscribe(nameof(slider.ValueChanged), HandleSliderValueChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing)
                return;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}
