// MvxUIButtonTitleTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUIButtonTitleTargetBinding : MvxConvertingTargetBinding
    {
        private readonly UIControlState _state;

        public MvxUIButtonTitleTargetBinding(UIButton button, UIControlState state = UIControlState.Normal)
            : base(button)
        {
            _state = state;
            if (button == null)
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UIButton is null in MvxUIButtonTitleTargetBinding");
        }

        protected UIButton Button => Target as UIButton;

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            ((UIButton) target).SetTitle(value as string, _state);
        }
    }
}