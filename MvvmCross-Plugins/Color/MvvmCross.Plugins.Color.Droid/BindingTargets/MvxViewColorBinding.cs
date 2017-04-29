// MvxViewColorBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;

namespace MvvmCross.Plugins.Color.Droid.BindingTargets
{
    [Preserve(AllMembers = true)]
    public abstract class MvxViewColorBinding
        : MvxAndroidTargetBinding
    {
        protected MvxViewColorBinding(View view)
            : base(view)
        {
        }

        protected View TextView => (View) Target;

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(Android.Graphics.Color);
    }
}