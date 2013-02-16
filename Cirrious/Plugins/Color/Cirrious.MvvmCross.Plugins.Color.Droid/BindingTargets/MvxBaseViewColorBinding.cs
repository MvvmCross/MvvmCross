using System;
using Cirrious.MvvmCross.Binding.Droid.Target;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public abstract class MvxBaseViewColorBinding
        : MvxBaseAndroidTargetBinding
    {
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