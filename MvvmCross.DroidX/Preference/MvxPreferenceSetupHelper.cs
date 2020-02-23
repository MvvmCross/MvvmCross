// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using AndroidX.Preference;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.DroidX.Preference.Target;

namespace MvvmCross.DroidX.Preference
{
    public static class MvxPreferenceSetupHelper
    {
        public static void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<AndroidX.Preference.Preference>(
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

            registry.RegisterCustomBindingFactory<AndroidX.Preference.Preference>(
                MvxPreferencePropertyBinding.Preference_Click,
                preference => new MvxPreferenceClickTargetBinding(preference));
        }
    }
}
