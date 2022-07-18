// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Target
{
    public abstract class MvxBaseUIViewVisibleTargetBinding : MvxConvertingTargetBinding
    {
        protected UIView View => (UIView)Target;

        protected MvxBaseUIViewVisibleTargetBinding(UIView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetValueType => typeof(bool);
    }
}
