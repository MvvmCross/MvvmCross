using System;
using System.Windows.Input;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Target;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Droid.Support.V7.Preference.Target
{
    public class MvxPreferenceClickTargetBinding : MvxAndroidTargetBinding
    {
        private ICommand _command;
        private IDisposable _clickSubscription;
        private IDisposable _canExecuteSubscription;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;

        protected Android.Support.V7.Preferences.Preference Preference => (Android.Support.V7.Preferences.Preference)Target;

        public MvxPreferenceClickTargetBinding(Android.Support.V7.Preferences.Preference view)
            : base(view)
        {
            _canExecuteEventHandler = OnCanExecuteChanged;

            _clickSubscription = Preference.WeakSubscribe<Android.Support.V7.Preferences.Preference, Android.Support.V7.Preferences.Preference.PreferenceClickEventArgs>(
                nameof(Preference.PreferenceClick),
                ViewOnPreferenceClick);
        }

        private void ViewOnPreferenceClick(object sender, Android.Support.V7.Preferences.Preference.PreferenceClickEventArgs args)
        {
            if (_command == null)
                return;

            if (!_command.CanExecute(null))
                return;

            _command.Execute(null);
        }

        protected override void SetValueImpl(object target, object value)
        {
            _canExecuteSubscription?.Dispose();
            _canExecuteSubscription = null;

            _command = value as ICommand;
            if (_command != null)
            {
                _canExecuteSubscription = MvvmCross.WeakSubscription.MvxWeakSubscriptionExtensions.WeakSubscribe(_command, _canExecuteEventHandler);
            }
            RefreshEnabledState();
        }

        private void RefreshEnabledState()
        {
            var view = Preference;
            if (view == null)
                return;

            var shouldBeEnabled = false;
            if (_command != null)
            {
                shouldBeEnabled = _command.CanExecute(null);
            }
            view.Enabled = shouldBeEnabled;
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            RefreshEnabledState();
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(ICommand);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _clickSubscription?.Dispose();
                _clickSubscription = null;

                _canExecuteSubscription?.Dispose();
                _canExecuteSubscription = null;
            }
            base.Dispose(isDisposing);
        }
    }
}
