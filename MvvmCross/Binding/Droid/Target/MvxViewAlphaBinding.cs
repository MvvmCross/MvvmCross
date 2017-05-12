using System;
using Android.Views;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxViewAlphaBinding : MvxAndroidTargetBinding<View, float>
    {
        public MvxViewAlphaBinding(View target)
            : base(target)
        {
        }

        protected override void SetValueImpl(View target, float value)
        {
            target.Alpha = value;
        }
    }
}