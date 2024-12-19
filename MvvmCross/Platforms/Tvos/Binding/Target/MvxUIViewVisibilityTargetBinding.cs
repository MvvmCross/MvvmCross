// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.UI;

namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    public class MvxUIViewVisibilityTargetBinding : MvxConvertingTargetBinding
    {
        protected UIView View => (UIView)Target;

        public MvxUIViewVisibilityTargetBinding(UIView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetValueType => typeof(MvxVisibility);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIView)target;
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
                    MvxBindingLog.Instance?.LogWarning("Visibility out of range {Value}", value);
                    break;
            }
        }
    }
}
