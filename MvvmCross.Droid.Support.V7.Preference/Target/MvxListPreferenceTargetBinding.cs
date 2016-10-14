using Android.Support.V7.Preferences;

namespace MvvmCross.Droid.Support.V7.Preference.Target
{
    public class MvxListPreferenceTargetBinding : MvxPreferenceValueTargetBinding
    {
        public MvxListPreferenceTargetBinding(ListPreference preference)
            : base(preference)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var pref = target as ListPreference;
            if (pref != null)
                pref.Value = (string)value;
        }
    }
}