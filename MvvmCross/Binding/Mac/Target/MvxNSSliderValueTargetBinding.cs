// MvxUISliderValueTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using AppKit;
using Foundation;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Mac.Target
{
    public class MvxNSSliderValueTargetBinding : MvxPropertyInfoTargetBinding<NSSlider>
    {
        public MvxNSSliderValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var slider = View;
            if (slider == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - NSSlider is null in MvxNSSliderValueTargetBinding");
            }
            else
            {
                slider.Activated += HandleSliderActivation;
                //slider.Action = new MonoMac.ObjCRuntime.Selector ("sliderChanged:");
            }
        }

        [Export("sliderChanged:")]
        private void HandleSliderActivation(object sender, EventArgs args)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.IntValue);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var slider = View;
                if (slider != null)
                {
                    slider.Activated -= HandleSliderActivation;
                }
            }
        }
    }
}