// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    public class MvxUIViewLayerBorderWidthTargetBinding
        : MvxConvertingTargetBinding
    {
        public MvxUIViewLayerBorderWidthTargetBinding(object target)
            : base(target)
        {
        }

        public override Type TargetValueType => typeof(float);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIView)target;

            if (view == null || value == null)
                return;

            if (view.Layer == null)
                return;

            view.Layer.BorderWidth = (float)value;
        }
    }
}
