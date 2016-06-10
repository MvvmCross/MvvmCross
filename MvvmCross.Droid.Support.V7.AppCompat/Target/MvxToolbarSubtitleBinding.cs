using System;
using Android.Support.V7.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    public class MvxToolbarSubtitleBinding : MvxConvertingTargetBinding
    {
        public MvxToolbarSubtitleBinding(Toolbar toolbar) : base(toolbar) {}

        public override Type TargetType => typeof(string);
        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            ((Toolbar)target).Subtitle = (string)value;
        }

        protected Toolbar Toolbar => (Toolbar)this.Target;
    }
}