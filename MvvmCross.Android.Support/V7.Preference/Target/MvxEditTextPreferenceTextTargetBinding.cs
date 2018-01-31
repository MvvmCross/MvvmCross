// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Support.V7.Preferences;

namespace MvvmCross.Droid.Support.V7.Preference.Target
{
    public class MvxEditTextPreferenceTextTargetBinding 
        : MvxPreferenceValueTargetBinding
    {
        public MvxEditTextPreferenceTextTargetBinding(EditTextPreference preference)
            : base(preference)
        {
        }

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