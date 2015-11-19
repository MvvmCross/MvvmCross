// MvxUIActivityIndicatorViewHiddenTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    /// <summary>
    /// Custom binding for UIActivityIndicator hidden. 
    /// This binding will ensure the indicator animates when shown and stops when hidden
    /// </summary>
    public class MvxUIActivityIndicatorViewHiddenTargetBinding : MvxConvertingTargetBinding
    {
        public MvxUIActivityIndicatorViewHiddenTargetBinding(UIActivityIndicatorView target)
            : base(target)
        {
            if (target == null)
            {
                MvxBindingTrace.Trace(
                                    MvxTraceLevel.Error,
                                    "Error - UIActivityIndicatorView is null in MvxUIActivityIndicatorViewHiddenTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override System.Type TargetType
        {
            get { return typeof(bool); }
        }

        protected UIActivityIndicatorView View
        {
            get { return Target as UIActivityIndicatorView; }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIActivityIndicatorView)target;
            if (view == null)
            {
                return;
            }

            view.Hidden = (bool)value;

            if (view.Hidden)
            {
                view.StopAnimating();
            }
            else
            {
                view.StartAnimating();
            }
        }
    }
}
