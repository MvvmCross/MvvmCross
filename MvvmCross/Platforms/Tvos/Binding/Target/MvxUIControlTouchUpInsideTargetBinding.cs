// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    public class MvxUIControlTouchUpInsideTargetBinding : MvxConvertingTargetBinding
    {
        private ICommand _command;
        private IDisposable _canExecuteSubscription;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;

        protected UIControl Control => Target as UIControl;

        public MvxUIControlTouchUpInsideTargetBinding(UIControl control)
            : base(control)
        {
            if (control == null)
            {
                MvxBindingLog.Error("Error - UIControl is null in MvxUIControlTouchUpInsideTargetBinding");
            }
            else
            {
                control.PrimaryActionTriggered += ControlOnTouchUpInside;
            }

            _canExecuteEventHandler = new EventHandler<EventArgs>(OnCanExecuteChanged);
        }

        private void ControlOnTouchUpInside(object sender, EventArgs eventArgs)
        {
            if (_command == null)
                return;

            if (!_command.CanExecute(null))
                return;

            _command.Execute(null);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetValueType => typeof(ICommand);

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

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var view = Control;
                if (view != null)
                {
                    view.PrimaryActionTriggered -= ControlOnTouchUpInside;
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
