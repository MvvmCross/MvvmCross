// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.UI.Xaml;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.WinUi.Binding.MvxBinding.Target
{
    public class MvxVisibleTargetBinding : MvxDependencyPropertyTargetBinding
    {
        public MvxVisibleTargetBinding(object target)
            : base(target, "Visibility", UIElement.VisibilityProperty, typeof(Visibility))
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;
        
        public override void SetValue(object value)
        {
            if (value == null)
                value = false;
            var boolValue = (bool)value;
            base.SetValue(boolValue ? Visibility.Visible : Visibility.Collapsed);
        }
    }
}
