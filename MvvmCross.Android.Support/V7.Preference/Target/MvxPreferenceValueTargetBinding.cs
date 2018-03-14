// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Binding;
using MvvmCross.Platform.Android.Binding.Target;

namespace MvvmCross.Droid.Support.V7.Preference.Target
{
    public class MvxPreferenceValueTargetBinding : MvxAndroidTargetBinding
    {
        public MvxPreferenceValueTargetBinding(Android.Support.V7.Preferences.Preference preference)
            : base(preference)
        {
        }

        public Android.Support.V7.Preferences.Preference Preference => Target as Android.Support.V7.Preferences.Preference;

        public override Type TargetType => typeof(Android.Support.V7.Preferences.Preference);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            Preference.PreferenceChange += HandlePreferenceChange;
        }

        protected void HandlePreferenceChange(object sender, Android.Support.V7.Preferences.Preference.PreferenceChangeEventArgs e)
        {
            if (e.Preference == Preference)
            {
                this.FireValueChanged(e.NewValue);
                e.Handled = true;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (Preference != null)
                {
                    Preference.PreferenceChange -= HandlePreferenceChange;
                }
            }

            base.Dispose(isDisposing);
        }

        protected override void SetValueImpl(object target, object value)
        {
            MvxBindingLog.Warn("SetValueImpl called on generic Preference target");
        }
    }
}
