// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using AndroidX.Preference;

namespace MvvmCross.DroidX.Preference
{
    public static class MvxPreferencePropertyBindingExtensions
    {
        public static string BindValue(this AndroidX.Preference.Preference preference)
            => MvxPreferencePropertyBinding.Preference_Value;

        public static string BindText(this EditTextPreference editTextPreference)
            => MvxPreferencePropertyBinding.EditTextPreference_Text;

        public static string BindValue(this ListPreference listPreference)
            => MvxPreferencePropertyBinding.ListPreference_Value;

        public static string BindChecked(this TwoStatePreference twoStatePreference)
            => MvxPreferencePropertyBinding.TwoStatePreference_Checked;

        public static string BindClick(this AndroidX.Preference.Preference preference)
            => MvxPreferencePropertyBinding.Preference_Click;
    }
}
