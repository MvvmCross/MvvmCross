using System;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Target;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public abstract class MvxBaseViewColorBinding
        : MvxBaseAndroidTargetBinding
    {
        protected View TextView
        {
            get { return (View)base.Target; }
        }

        protected MvxBaseViewColorBinding(View view)
            : base(view)
        {
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override Type TargetType
        {
            get { return typeof(Android.Graphics.Color); }
        }
    }
}