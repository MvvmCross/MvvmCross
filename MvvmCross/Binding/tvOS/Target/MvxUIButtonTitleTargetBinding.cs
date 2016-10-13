// MvxUIButtonTitleTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.tvOS.Target
{
    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Platform.Platform;

    using UIKit;

    public class MvxUIButtonTitleTargetBinding : MvxConvertingTargetBinding
    {
        private readonly UIControlState _state;

        protected UIButton Button => base.Target as UIButton;

        public MvxUIButtonTitleTargetBinding(UIButton button, UIControlState state = UIControlState.Normal)
            : base(button)
        {
            this._state = state;
            if (button == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UIButton is null in MvxUIButtonTitleTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override System.Type TargetType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            ((UIButton)target).SetTitle(value as string, this._state);
        }
    }
}