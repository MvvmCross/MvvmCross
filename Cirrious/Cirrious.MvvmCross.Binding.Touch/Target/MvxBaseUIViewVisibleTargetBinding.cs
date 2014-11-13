// MvxBaseUIViewVisibleTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public abstract class MvxBaseUIViewVisibleTargetBinding : MvxConvertingTargetBinding
    {
        protected UIView View
        {
            get { return (UIView)Target; }
        }

        protected MvxBaseUIViewVisibleTargetBinding(UIView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override System.Type TargetType
        {
            get { return typeof(bool); }
        }
    }
}