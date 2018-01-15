// MvxViewClickBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using Android.Views;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxViewClickBinding : MvxAndroidTargetBinding
    {
        private ICommand _command;
        private IDisposable _clickSubscription;
        private IDisposable _canExecuteSubscription;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;

        protected View View => (View)Target;

        public MvxViewClickBinding(View view)
            : base(view)
        {
            _canExecuteEventHandler = OnCanExecuteChanged;
            _clickSubscription = view.WeakSubscribe(nameof(view.Click), ViewOnClick);
        }

        private void ViewOnClick(object sender, EventArgs args)
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
                _canExecuteSubscription = _command.WeakSubscribe(_canExecuteEventHandler);
            }
            RefreshEnabledState();
        }

        private void RefreshEnabledState()
        {
            var view = View;
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