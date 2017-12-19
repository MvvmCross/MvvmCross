// MvxPreferencePropertyBindingExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Support.V7.Preferences;

namespace MvvmCross.Droid.Support.V7.Preference
{
    public static class MvxPreferencePropertyBindingExtensions
    {
        public static string BindValue(this Android.Support.V7.Preferences.Preference preference)
            => MvxPreferencePropertyBinding.Preference_Value;

        public static string BindText(this EditTextPreference editTextPreference)
            => MvxPreferencePropertyBinding.EditTextPreference_Text;

        public static string BindValue(this ListPreference listPreference)
            => MvxPreferencePropertyBinding.ListPreference_Value;

        public static string BindChecked(this TwoStatePreference twoStatePreference)
            => MvxPreferencePropertyBinding.TwoStatePreference_Checked;
    }
}
