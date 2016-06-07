using System;
using Android.Widget;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxTextViewHintTargetBinding : MvxAndroidTargetBinding
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