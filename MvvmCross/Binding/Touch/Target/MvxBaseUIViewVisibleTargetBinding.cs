// MvxBaseUIViewVisibleTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Target
{
    using MvvmCross.Binding.Bindings.Target;

    using UIKit;

    public abstract class MvxBaseUIViewVisibleTargetBinding : MvxConvertingTargetBinding
    {
        protected UIView View => (UIView)Target;

        protected MvxBaseUIViewVisibleTargetBinding(UIView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override System.Type TargetType => typeof(bool);
    }
}