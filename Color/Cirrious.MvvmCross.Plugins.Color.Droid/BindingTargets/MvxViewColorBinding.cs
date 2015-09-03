// MvxViewColorBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Views;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Droid.Target;

namespace MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public abstract class MvxViewColorBinding
        : MvxAndroidTargetBinding
    {
        protected View TextView
        {
            get { return (View) base.Target; }
        }

        protected MvxViewColorBinding(View view)
            : base(view)
        {
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override Type TargetType
        {
            get { return typeof (Android.Graphics.Color); }
        }
    }
}