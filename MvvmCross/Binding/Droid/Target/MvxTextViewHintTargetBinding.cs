using System;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxTextViewHintTargetBinding 
        : MvxConvertingTargetBinding
    {
        public MvxTextViewHintTargetBinding(TextView target)
            : base(target) {}

        public override Type TargetType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            ((TextView)target).Hint = (string)value;
        }
    }
}