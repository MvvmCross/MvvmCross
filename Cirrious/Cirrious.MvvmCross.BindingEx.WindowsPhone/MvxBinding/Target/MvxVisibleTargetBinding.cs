// MvxVisibleTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows;

namespace Cirrious.MvvmCross.BindingEx.WindowsPhone.MvxBinding.Target
{
    public class MvxVisibleTargetBinding : MvxDependencyPropertyTargetBinding
    {
        public MvxVisibleTargetBinding(object target)
            : base(target, "Visibility", UIElement.VisibilityProperty)
        {
        }

        public override Type TargetType
        {
            get { return typeof (bool); }
        }

        public override void SetValue(object value)
        {
            var boolValue = (bool) value;
            base.SetValue(boolValue ? Visibility.Visible : Visibility.Collapsed);
        }
    }
}