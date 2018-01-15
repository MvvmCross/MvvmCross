﻿// MvxUIViewTapTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.tvOS.Views.Gestures;
using UIKit;

namespace MvvmCross.Binding.tvOS.Target
{
    public class MvxUIViewTapTargetBinding : MvxConvertingTargetBinding
    {
        private readonly MvxTapGestureRecognizerBehaviour _behaviour;

        public MvxUIViewTapTargetBinding(UIView target, uint numberOfTapsRequired = 1, uint numberOfTouchesRequired = 1, bool cancelsTouchesInView = true)
            : base(target)
        {
            _behaviour = new MvxTapGestureRecognizerBehaviour(target, numberOfTapsRequired, numberOfTouchesRequired, cancelsTouchesInView);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(ICommand);

        protected override void SetValueImpl(object target, object value)
        {
            _behaviour.Command = (ICommand)value;
        }
    }
}