// MvxUIViewHiddenTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Target
{
    using MvvmCross.Binding.ExtensionMethods;

    using UIKit;

    public class MvxUIViewHiddenTargetBinding : MvxBaseUIViewVisibleTargetBinding
    {
        public MvxUIViewHiddenTargetBinding(UIView target)
            : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = this.View;
            if (view == null)
                return;

            var hidden = value.ConvertToBoolean();
            view.Hidden = hidden;
        }
    }
}