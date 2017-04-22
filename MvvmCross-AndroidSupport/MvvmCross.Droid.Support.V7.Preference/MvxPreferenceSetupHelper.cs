// MvxPreferenceSetupHelper.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.Preference.Target;

namespace MvvmCross.Droid.Support.V7.Preference
{
    public static class MvxPreferenceSetupHelper
    {
        public static void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<Android.Support.V7.Preferences.Preference>(
                MvxPreferencePropertyBinding.Preference_Value,
                preference => new MvxPreferenceValueTargetBinding(preference));

            registry.RegisterCustomBindingFactory<Android.Support.V7.Preferences.EditTextPreference>(
                MvxPreferencePropertyBinding.EditTextPreference_Text,
                preference => new MvxEditTextPreferenceTextTargetBinding(preference));

            registry.RegisterCustomBindingFactory<Android.Support.V7.Preferences.ListPreference>(
                MvxPreferencePropertyBinding.ListPreference_Value,
                preference => new MvxListPreferenceTargetBinding(preference));

            registry.RegisterCustomBindingFactory<Android.Support.V7.Preferences.TwoStatePreference>(
                MvxPreferencePropertyBinding.TwoStatePreference_Checked,
                preference => new MvxTwoStatePreferenceCheckedTargetBinding(preference));
        }
    }
}
