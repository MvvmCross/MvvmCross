// MvxUIViewHiddenTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.ExtensionMethods;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUIViewHiddenTargetBinding : MvxBaseUIViewVisibleTargetBinding
    {
        public MvxUIViewHiddenTargetBinding(UIView target)
            : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = View;
            if (view == null)
                return;

            var hidden = value.ConvertToBoolean();
            view.Hidden = hidden;
        }
    }
}