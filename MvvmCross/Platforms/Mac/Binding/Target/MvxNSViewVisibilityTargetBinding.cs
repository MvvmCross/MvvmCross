// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using MvvmCross.Binding;
using MvvmCross.UI;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public class MvxNSViewVisibilityTargetBinding : MvxMacTargetBinding
    {
        protected NSView View
        {
            get { return (NSView)Target; }
        }

        public MvxNSViewVisibilityTargetBinding(NSView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override Type TargetValueType
        {
            get { return typeof(MvxVisibility); }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = this.View;
            if (view == null)
                return;

            var visibility = (MvxVisibility)value;
            switch (visibility)
            {
                case MvxVisibility.Visible:
                    view.Hidden = false;
                    break;

                case MvxVisibility.Collapsed:
                    view.Hidden = true;
                    break;

                default:
                    MvxBindingLog.Warning("Visibility out of range {0}", value);
                    break;
            }
        }
    }
}
