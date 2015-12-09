// MvxVisibleTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

#if WINDOWS_PHONE || WINDOWS_WPF

using System.Windows;

#endif
#if NETFX_CORE

using Windows.UI.Xaml;

#endif

// ReSharper disable CheckNamespace
namespace Cirrious.MvvmCross.BindingEx.WindowsShared.MvxBinding.Target
// ReSharper restore CheckNamespace
{
    public class MvxVisibleTargetBinding : MvxDependencyPropertyTargetBinding
    {
        public MvxVisibleTargetBinding(object target)
            : base(target, "Visibility", UIElement.VisibilityProperty, typeof(Visibility))
        {
        }

        public override Binding.MvxBindingMode DefaultMode => Binding.MvxBindingMode.OneWay;

        public override Type TargetType => typeof(bool);

        public override void SetValue(object value)
        {
            if (value == null)
                value = false;
            var boolValue = (bool)value;
            base.SetValue(boolValue ? Visibility.Visible : Visibility.Collapsed);
        }
    }
}