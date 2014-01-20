using System;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUIViewLayerBorderWidthTargetBinding
        : MvxConvertingTargetBinding
    {
        public MvxUIViewLayerBorderWidthTargetBinding(object target) 
            : base(target)
        {
        }

        public override Type TargetType
        {
            get { return typeof (UIView); }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIView)target;

            if (view == null || value == null)
                return;

            view.Layer.BorderWidth = (float)value;
        }
    }
}