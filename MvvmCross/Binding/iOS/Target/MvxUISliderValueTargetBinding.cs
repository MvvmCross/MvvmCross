// MvxUISliderValueTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUISliderValueTargetBinding
        : MvxPropertyInfoTargetBinding<UISlider>
    {
        private bool _subscribed;

        public MvxUISliderValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as UISlider;
            if (view == null)
                return;

            view.Value = (float)value;
        }

        private void HandleSliderValueChanged(object sender, EventArgs e)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.Value);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var slider = View;
            if (slider == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UISlider is null in MvxUISliderValueTargetBinding");
                return;
            }

            _subscribed = true;
            slider.ValueChanged += HandleSliderValueChanged;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var slider = View;
                if (slider != null && _subscribed)
                {
                    slider.ValueChanged -= HandleSliderValueChanged;
                    _subscribed = false;
                }
            }
        }
    }
}