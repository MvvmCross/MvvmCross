﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.Extensions;
using UIKit;

namespace MvvmCross.Platform.Ios.Binding.Target
{
    public class MvxUIViewHiddenTargetBinding : MvxBaseUIViewVisibleTargetBinding
    {
        public MvxUIViewHiddenTargetBinding(UIView target)
            : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = View;
            if (view == null) return;

            var hidden = value.ConvertToBoolean();
            view.Hidden = hidden;
        }
    }
}
