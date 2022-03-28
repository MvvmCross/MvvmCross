// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AndroidX.Preference;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxPreferenceValueTargetBinding
        : MvxAndroidTargetBinding
    {
        private IDisposable _subscription;

        public MvxPreferenceValueTargetBinding(Preference preference)
            : base(preference)
        {
        }

        public Preference Preference => Target as Preference;

        public override Type TargetValueType => typeof(Preference);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            _subscription = Preference.WeakSubscribe<Preference, Preference.PreferenceChangeEventArgs>(
                nameof(Preference.PreferenceChange),
                HandlePreferenceChange);
        }

        protected void HandlePreferenceChange(object sender, Preference.PreferenceChangeEventArgs e)
        {
            if (e.Preference == Preference)
            {
                FireValueChanged(e.NewValue);
                e.Handled = true;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }

            base.Dispose(isDisposing);
        }

        protected override void SetValueImpl(object target, object value)
        {
            MvxBindingLog.Warning("SetValueImpl called on generic Preference target");
        }
    }
}
