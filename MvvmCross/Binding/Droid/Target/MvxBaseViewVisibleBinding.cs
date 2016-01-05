// MvxBaseViewVisibleBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
    using System;

    using Android.Views;

    public abstract class MvxBaseViewVisibleBinding
        : MvxAndroidTargetBinding
    {
        protected View View => (View)Target;

        protected MvxBaseViewVisibleBinding(object target)
            : base(target)
        {
        }

        public override Type TargetType => typeof(bool);
    }
}