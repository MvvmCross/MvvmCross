// MvxCollapsedTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


namespace MvvmCross.BindingEx.WindowsPhone.MvxBinding.Target

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