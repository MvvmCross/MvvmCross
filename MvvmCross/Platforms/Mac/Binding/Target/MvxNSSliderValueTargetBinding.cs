// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using AppKit;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public class MvxNSSliderValueTargetBinding : MvxPropertyInfoTargetBinding<NSSlider>
    {
        public MvxNSSliderValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var slider = View;
            if (slider == null)
            {
                MvxBindingLog.Error("Error - NSSlider is null in MvxNSSliderValueTargetBinding");
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
