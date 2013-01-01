// MvxUIButtonTitleTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUIButtonTitleTargetBinding : MvxBaseTargetBinding
    {
        private readonly UIButton _button;

        public MvxUIButtonTitleTargetBinding(UIButton button)
        {
            _button = button;
            if (_button == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UIButton is null in MvxUIButtonTitleTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override System.Type TargetType
        {
            get { return typeof (string); }
        }

        public override void SetValue(object value)
        {
            if (_button == null)
                return;

            _button.SetTitle(value as string, UIControlState.Normal);
        }
    }
}