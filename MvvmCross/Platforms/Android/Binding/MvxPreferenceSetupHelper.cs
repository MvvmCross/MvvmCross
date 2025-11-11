using System.Diagnostics.CodeAnalysis;
using AndroidX.Preference;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Android.Binding.Target;

namespace MvvmCross.Platforms.Android.Binding
{
    public static class MvxPreferenceSetupHelper
    {
        [RequiresUnreferencedCode("This method may use types that are not preserved by trimming")]
        public static void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<Preference>(
                MvxPreferencePropertyBinding.Preference_Value,
                preference => new MvxPreferenceValueTargetBinding(preference));

            registry.RegisterCustomBindingFactory<EditTextPreference>(
                MvxPreferencePropertyBinding.EditTextPreference_Text,
                preference => new MvxEditTextPreferenceTextTargetBinding(preference));

            registry.RegisterCustomBindingFactory<ListPreference>(
                MvxPreferencePropertyBinding.ListPreference_Value,
                preference => new MvxListPreferenceTargetBinding(preference));

            registry.RegisterCustomBindingFactory<TwoStatePreference>(
                MvxPreferencePropertyBinding.TwoStatePreference_Checked,
                preference => new MvxTwoStatePreferenceCheckedTargetBinding(preference));

            registry.RegisterCustomBindingFactory<Preference>(
                MvxPreferencePropertyBinding.Preference_Click,
                preference => new MvxPreferenceClickTargetBinding(preference));
        }
    }
}
