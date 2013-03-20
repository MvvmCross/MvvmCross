// MvxUIViewVisibilityTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.UI;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUIViewVisibilityTargetBinding : MvxTargetBinding
    {
        protected UIView View
        {
            get { return (UIView) Target; }
        }

        public MvxUIViewVisibilityTargetBinding(UIView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override System.Type TargetType
        {
            get { return typeof (MvxVisibility); }
        }

        public override void SetValue(object value)
        {
            var view = View;
            if (view == null)
                return;

            var visibility = (MvxVisibility) value;
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