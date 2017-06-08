// MvxBaseUIViewVisibleTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Binding.iOS.Target
{
    using Bindings.Target;

    using UIKit;

    public abstract class MvxBaseUIViewVisibleTargetBinding : MvxConvertingTargetBinding
    {
        protected UIView View => (UIView)Target;

        protected MvxBaseUIViewVisibleTargetBinding(UIView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(bool);
    }
}