// MvxUIActivityIndicatorViewHiddenTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Target
{
    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Platform.Platform;

    using UIKit;

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

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override System.Type TargetType => typeof(bool);

        protected UIActivityIndicatorView View => Target as UIActivityIndicatorView;

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