// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platforms.Tvos.Binding.Views.Gestures;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Target
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

        public override Type TargetValueType => typeof(ICommand);

        protected override void SetValueImpl(object target, object value)
        {
            _behaviour.Command = (ICommand)value;
        }
    }
}
