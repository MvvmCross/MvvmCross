// MvxUIControlValueChangedTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt

using System;
using System.Windows.Input;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using UIKit;

namespace MvvmCross.Binding.tvOS.Target
{
    public class MvxUIControlValueChangedTargetBinding
        : MvxConvertingTargetBinding
    {
        private ICommand _command;
        private EventHandler<EventArgs> _canExecuteEventHandler;
        private MvxCanExecuteChangedEventSubscription _canExecuteSubscription;

        protected UIControl Control => Target as UIControl;

        public MvxUIControlValueChangedTargetBinding(UIControl control)
            : base(control)
        {
            if (control == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                    "Error - UIControl is null in MvxUIControlValueChangedTargetBinding");
            }
            else
            {
                control.ValueChanged += OnValueChanged;
            }

            _canExecuteEventHandler = OnCanExecuteChanged;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (!_command?.CanExecute(null) ?? true)
                return;

            _command?.Execute(null);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;
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

        private void RefreshEnabledState()
        {
            var view = Control;
            if (view == null)
                return;

            view.Enabled = _command?.CanExecute(null) ?? false;
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            RefreshEnabledState();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var view = Control;
                if (view != null)
                {
                    view.ValueChanged -= OnValueChanged;
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