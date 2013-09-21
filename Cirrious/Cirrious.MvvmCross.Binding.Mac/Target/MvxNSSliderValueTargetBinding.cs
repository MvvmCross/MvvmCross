// MvxUISliderValueTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace Cirrious.MvvmCross.Binding.Mac.Target
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
				slider.Action = new MonoMac.ObjCRuntime.Selector ("sliderAction:");
            }
        }

		[Export("sliderAction:")]
		private void sliderAction()
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
//                    slider.ValueChanged -= HandleSliderValueChanged;
                }
            }
        }
    }
}