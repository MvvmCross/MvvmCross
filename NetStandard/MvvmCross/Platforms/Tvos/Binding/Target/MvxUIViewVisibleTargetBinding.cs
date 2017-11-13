// MvxUIViewVisibleTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding.ExtensionMethods;
using UIKit;

namespace MvvmCross.Binding.tvOS.Target
{
    public class MvxUIViewVisibleTargetBinding : MvxBaseUIViewVisibleTargetBinding
    {
        public MvxUIViewVisibleTargetBinding(UIView target)
            : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = View;
            if (view == null)
                return;

            var visible = value.ConvertToBoolean();
            view.Hidden = !visible;
        }
    }
}