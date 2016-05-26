using System;
using Android.Preferences;
using MvvmCross.Platform;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxPreferenceValueTargetBinding : MvxAndroidTargetBinding
    {
        public MvxPreferenceValueTargetBinding(Preference preference)
            : base(preference)
        { }

        public Preference Preference => Target as Preference;

        public override Type TargetType => typeof(Preference);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            Preference.PreferenceChange += HandlePreferenceChange;
        }

        protected void HandlePreferenceChange(object sender, Preference.PreferenceChangeEventArgs e)
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