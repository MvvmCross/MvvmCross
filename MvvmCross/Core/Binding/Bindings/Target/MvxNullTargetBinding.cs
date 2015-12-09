// MvxNullTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target
{
    using System;

    public class MvxNullTargetBinding : MvxTargetBinding
    {
        public MvxNullTargetBinding()
            : base(null)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneTime;

        public override Type TargetType => typeof(Object);

        public override void SetValue(object value)
        {
            // ignored
        }
    }
}