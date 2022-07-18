// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Target
{
    public class MvxUILabelTextTargetBinding
        : MvxConvertingTargetBinding
    {
        protected UILabel View => Target as UILabel;

        public MvxUILabelTextTargetBinding(UILabel target)
            : base(target)
        {
            if (target == null)
            {
                MvxBindingLog.Error(
                                      "Error - UILabel is null in MvxUILabelTextTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UILabel)target;
            if (view == null)
                return;

            view.Text = (string)value;
        }
    }
}
