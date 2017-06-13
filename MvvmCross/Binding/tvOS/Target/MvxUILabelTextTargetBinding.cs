﻿// MvxUILabelTextTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.Binding.tvOS.Target
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
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - UILabel is null in MvxUILabelTextTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UILabel)target;
            if (view == null)
                return;

            view.Text = (string)value;
        }
    }
}