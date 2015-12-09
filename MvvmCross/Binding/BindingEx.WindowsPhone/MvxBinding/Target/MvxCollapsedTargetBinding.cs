// MvxCollapsedTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

// ReSharper disable CheckNamespace
namespace Cirrious.MvvmCross.BindingEx.WindowsShared.MvxBinding.Target
// ReSharper restore CheckNamespace
{
    public class MvxCollapsedTargetBinding : MvxVisibleTargetBinding
    {
        public MvxCollapsedTargetBinding(object target)
            : base(target)
        {
        }

        public override void SetValue(object value)
        {
            if (value == null)
                value = false;
            var boolValue = (bool)value;
            base.SetValue(!boolValue);
        }
    }
}