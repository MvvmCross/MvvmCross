using Android.Preferences;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxTwoStatePreferenceCheckedTargetBinding : MvxPreferenceValueTargetBinding
    {
        public MvxTwoStatePreferenceCheckedTargetBinding(TwoStatePreference preference)
            : base(preference) { }

        protected override void SetValueImpl(object target, object value)
        {
            var t = target as TwoStatePreference;
            if (t != null)
            {
                t.Checked = (bool)value;
            }
        }
    }
}