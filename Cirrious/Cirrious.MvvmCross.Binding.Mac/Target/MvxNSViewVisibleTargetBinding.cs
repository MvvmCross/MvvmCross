// MvxUIViewVisibleTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Contributed by Tim Uy, tim@loqu8.com

#if __UNIFIED__
using AppKit;
#else
#endif

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
    public class MvxNSViewVisibleTargetBinding : MvxMacTargetBinding
    {
        protected NSView View
        {
            get { return (NSView)Target; }
        }

        public MvxNSViewVisibleTargetBinding(NSView target)
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

        protected override void SetValueImpl(object target, object value)
        {
            var view = View;
            if (view == null)
                return;

            var visible = (bool)value;
            view.Hidden = !visible;
        }
    }
}