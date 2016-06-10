// MvxBaseViewVisibleBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Binding.Droid.Target
{
    public abstract class MvxBaseViewVisibleBinding
        : MvxConvertingTargetBinding
    {
        protected View View => (View)Target;

        protected MvxBaseViewVisibleBinding(object target)
            : base(target)
        {
        }

        public override Type TargetType => typeof(bool);
    }
}