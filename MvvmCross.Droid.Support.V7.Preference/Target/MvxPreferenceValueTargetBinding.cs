using System;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform;

namespace MvvmCross.Droid.Support.V7.Preference.Target
{
    public class MvxPreferenceValueTargetBinding : MvxConvertingTargetBinding
    {
        public MvxPreferenceValueTargetBinding(Android.Support.V7.Preferences.Preference preference)
            : base(preference)
        { }

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
                    Preference.PreferenceChange -= this.HandlePreferenceChange;
                }
            }

            base.Dispose(isDisposing);
        }

        protected override void SetValueImpl(object target, object value)
        {
            Mvx.Warning("SetValueImpl called on generic Preference target");
        }
    }
}