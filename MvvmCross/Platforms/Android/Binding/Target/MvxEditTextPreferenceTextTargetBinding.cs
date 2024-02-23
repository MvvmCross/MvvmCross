// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using AndroidX.Preference;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxEditTextPreferenceTextTargetBinding(EditTextPreference preference)
    : MvxPreferenceValueTargetBinding(preference)
{
    protected override void SetValueImpl(object target, object? value)
    {
        if (target is EditTextPreference t)
        {
            t.Text = (string?)value;
        }
    }
}
