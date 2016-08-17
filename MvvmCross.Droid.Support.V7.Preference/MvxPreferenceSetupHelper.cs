using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.Preference.Target;

namespace MvvmCross.Droid.Support.V7.Preference
{
    public static class MvxPreferenceSetupHelper
    {
        public static void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<Android.Support.V7.Preferences.Preference>(
                "Value",
                preference => new MvxPreferenceValueTargetBinding(preference));

            registry.RegisterCustomBindingFactory<Android.Support.V7.Preferences.EditTextPreference>(
                "Text",
                preference => new MvxEditTextPreferenceTextTargetBinding(preference));

            registry.RegisterCustomBindingFactory<Android.Support.V7.Preferences.TwoStatePreference>(
                "Checked",
                preference => new MvxTwoStatePreferenceCheckedTargetBinding(preference));
        }
    }
}