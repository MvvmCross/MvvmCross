// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Mac.Binding.Target
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
                MvxBindingLog.Error("Error - NSButton is null in MvxNSButtonTitleTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override Type TargetValueType
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
