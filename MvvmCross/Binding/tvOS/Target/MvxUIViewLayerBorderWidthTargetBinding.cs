// MvxUIViewLayerBorderWidthTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.tvOS.Target
{
    using System;

    using MvvmCross.Binding.Bindings.Target;

    using UIKit;

    public class MvxUIViewLayerBorderWidthTargetBinding
        : MvxConvertingTargetBinding
    {
        public MvxUIViewLayerBorderWidthTargetBinding(object target)
            : base(target)
        {
        }

        public override Type TargetType => typeof(float);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIView)target;

            if (view == null || value == null)
                return;

            if (view.Layer == null)
                return;

            view.Layer.BorderWidth = (float)value;
        }
    }
}