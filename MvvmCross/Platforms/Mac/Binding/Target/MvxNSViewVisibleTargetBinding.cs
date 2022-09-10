// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public class MvxNSViewVisibleTargetBinding : MvxMacTargetBinding
    {
        protected NSView View
        {
            get { return (NSView)Target; }
        }

        public MvxNSViewVisibleTargetBinding(NSView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override Type TargetValueType
        {
            get { return typeof(bool); }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = this.View;
            if (view == null)
                return;

            var visible = (bool)value;
            view.Hidden = !visible;
        }
    }
}
