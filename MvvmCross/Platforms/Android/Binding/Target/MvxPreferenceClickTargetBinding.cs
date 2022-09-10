using System;
using System.Windows.Input;
using AndroidX.Preference;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxPreferenceClickTargetBinding : MvxAndroidTargetBinding
    {
        private ICommand _command;
        private IDisposable _clickSubscription;
        private IDisposable _canExecuteSubscription;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;

        protected Preference Preference => (Preference)Target;

        public MvxPreferenceClickTargetBinding(Preference view)
            : base(view)
        {
            _canExecuteEventHandler = OnCanExecuteChanged;

            _clickSubscription = Preference.WeakSubscribe<Preference, Preference.PreferenceClickEventArgs>(
                nameof(Preference.PreferenceClick),
                ViewOnPreferenceClick);
        }

        private void ViewOnPreferenceClick(object sender, Preference.PreferenceClickEventArgs args)
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

            view.Enabled = _command?.CanExecute(null) ?? false;
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            RefreshEnabledState();
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetValueType => typeof(ICommand);

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
