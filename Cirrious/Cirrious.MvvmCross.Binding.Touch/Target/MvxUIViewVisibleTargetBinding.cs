// MvxUIViewVisibleTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUIViewVisibleTargetBinding : MvxTargetBinding
    {
        protected UIView View
        {
            get { return (UIView) Target; }
        }

        public MvxUIViewVisibleTargetBinding(UIView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override System.Type TargetType
        {
            get { return typeof (bool); }
        }

        public override void SetValue(object value)
        {
            var view = View;
            if (view == null)
                return;

            var visible = (bool) value;
            view.Hidden = !visible;
        }
    }
}