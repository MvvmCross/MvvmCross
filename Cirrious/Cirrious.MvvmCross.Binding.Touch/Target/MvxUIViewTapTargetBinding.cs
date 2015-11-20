// MvxUIViewTapTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Touch.Views.Gestures;
using System;
using System.Windows.Input;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUIViewTapTargetBinding : MvxConvertingTargetBinding
    {
        private readonly MvxTapGestureRecognizerBehaviour _behaviour;

        public MvxUIViewTapTargetBinding(UIView target, uint numberOfTapsRequired = 1, uint numberOfTouchesRequired = 1)
            : base(target)
        {
            _behaviour = new MvxTapGestureRecognizerBehaviour(target, numberOfTapsRequired, numberOfTouchesRequired);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(ICommand);

        protected override void SetValueImpl(object target, object value)
        {
            _behaviour.Command = (ICommand)value;
        }
    }
}