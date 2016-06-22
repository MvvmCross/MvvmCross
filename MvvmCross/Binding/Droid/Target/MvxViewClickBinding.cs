// MvxViewClickBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using Android.Views;
using MvvmCross.Platform.WeakSubscription;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxViewClickBinding
        : MvxConvertingTargetBinding
    {
        private ICommand _command;
        private IDisposable _canExecuteSubscription;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;

        protected View View => (View)Target;

        public MvxViewClickBinding(View view)
            : base(view)
        {
            _canExecuteEventHandler = OnCanExecuteChanged;
            view.Click += ViewOnClick;
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
            if (_canExecuteSubscription != null)
            {
                _canExecuteSubscription.Dispose();
                _canExecuteSubscription = null;
            }
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
                var view = View;
                if (view != null)
                {
                    view.Click -= ViewOnClick;
                }
                if (_canExecuteSubscription != null)
                {
                    _canExecuteSubscription.Dispose();
                    _canExecuteSubscription = null;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}