﻿// MvxUIButtonTitleTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using AppKit;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Binding.Mac.Target
{
    public class MvxNSButtonTitleTargetBinding : MvxMacTargetBinding
    {
        protected NSButton Button
        {
            get { return base.Target as NSButton; }
        }

        public MvxNSButtonTitleTargetBinding(NSButton button)
            : base(button)
        {
            if (button == null)
            {
                MvxLog.Instance.Error("Error - NSButton is null in MvxNSButtonTitleTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override Type TargetType
        {
            get { return typeof(string); }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var button = this.Button;
            if (button == null)
                return;

            button.Title = value as string;
        }
    }
}