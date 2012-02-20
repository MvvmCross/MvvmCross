using System;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public class MvxNullTargetBinding : MvxBaseTargetBinding
    {
        public override void SetValue(object value)
        {
            // ignored
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneTime; }
        }

        public override Type TargetType
        {
            get { return typeof(Object); }
        }
    }
}