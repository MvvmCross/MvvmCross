// MvxNullTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Binding.Bindings.Target
{
    public class MvxNullTargetBinding : MvxTargetBinding
    {
        public MvxNullTargetBinding()
            : base(null)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneTime;

        public override Type TargetType => typeof(object);

        public override void SetValue(object value)
        {
            // ignored
        }
    }
}