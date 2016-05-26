using Android.Preferences;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxEditTextPreferenceTextTargetBinding : MvxPreferenceValueTargetBinding
    {
        public MvxEditTextPreferenceTextTargetBinding(EditTextPreference preference)
            : base(preference) { }

        protected override void SetValueImpl(object target, object value)
        {
            var t = target as EditTextPreference;
            if (t != null)
            {
                t.Text = (string)value;
            }
        }
    }
}