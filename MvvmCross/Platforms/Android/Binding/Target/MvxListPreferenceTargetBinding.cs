// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using AndroidX.Preference;

namespace MvvmCross.Platforms.Android.Binding.Target
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
