// MvxUISliderValueTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if __UNIFIED__
using AppKit;
using Foundation;
#else
#endif

namespace MvvmCross.Binding.Mac.Target
{
    using System.Reflection;

    using global::MvvmCross.Platform.Platform;

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
        private void HandleSliderActivation(object sender, System.EventArgs args)
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