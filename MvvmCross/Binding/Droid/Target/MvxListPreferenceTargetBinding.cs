using Android.Preferences;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxListPreferenceTargetBinding : MvxPreferenceValueTargetBinding
    {
        public MvxListPreferenceTargetBinding(Preference preference)
            : base(preference)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var pref = target as ListPreference;
            if (pref != null)
                pref.Value = (string) value;
        }
    }
}