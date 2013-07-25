// MvxUIViewTapTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Touch.Views.Gestures;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUIViewTapTargetBinding : MvxTargetBinding
    {
        private readonly MvxTapGestureRecognizerBehaviour _behaviour;

        public MvxUIViewTapTargetBinding(UIView target, uint numberOfTapsRequired = 1, uint numberOfTouchesRequired = 1)
            : base(target)
        {
            _behaviour = new MvxTapGestureRecognizerBehaviour(target, numberOfTapsRequired, numberOfTouchesRequired);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override Type TargetType
        {
            get { return typeof (ICommand); }
        }

        public override void SetValue(object value)
        {
            _behaviour.Command = (ICommand) value;
        }
    }
}