using System;
using System.Windows.Input;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUIBarButtonItemTargetBinding
        : MvxConvertingTargetBinding
    {
        private ICommand _command;
        private IDisposable _canExecuteSubscription;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;

        protected UIBarButtonItem Control => Target as UIBarButtonItem;

        public MvxUIBarButtonItemTargetBinding(UIBarButtonItem control)
            : base(control)
        {
            if (control == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UIControl is null in MvxUIBarButtonItemTargetBinding");
            }
            else
            {
                control.Clicked += OnClicked;
            }

            _canExecuteEventHandler = OnCanExecuteChanged;
        }

        private void OnClicked(object sender, EventArgs e)
        {
            if (_command == null)
                return;

            if (!_command.CanExecute(null))
                return;

            _command.Execute(null);
        }

        public override Type TargetType => typeof(ICommand);
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

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            RefreshEnabledState();
        }

        private void RefreshEnabledState()
        {
            var view = Control;
            if (view == null)
                return;

            var shouldBeEnabled = false;
            if (_command != null)
            {
                shouldBeEnabled = _command.CanExecute(null);
            }
            view.Enabled = shouldBeEnabled;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var view = Control;
                if (view != null)
                    view.Clicked -= OnClicked;
               
                _canExecuteSubscription?.Dispose();
                _canExecuteSubscription = null;
            }

            base.Dispose(isDisposing);
        }
    }
}