// MvxUIViewVisibilityTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Binding.tvOS.Target
{
    using Bindings.Target;
    using Platform.Platform;
    using Platform.UI;

    using UIKit;

    public class MvxUIViewVisibilityTargetBinding : MvxConvertingTargetBinding
    {
        protected UIView View => (UIView)Target;

        public MvxUIViewVisibilityTargetBinding(UIView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(MvxVisibility);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIView)target;
            var visibility = (MvxVisibility)value;
            switch (visibility)
            {
                case MvxVisibility.Visible:
                    view.Hidden = false;
                    break;

                case MvxVisibility.Collapsed:
                    view.Hidden = true;
                    break;

                default:
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Visibility out of range {0}", value);
                    break;
            }
        }
    }
}