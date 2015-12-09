// MvxUIControlTouchUpInsideTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using System;
using System.Windows.Input;
using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUIControlTouchUpInsideTargetBinding : MvxConvertingTargetBinding
    {
        private ICommand _command;
        private IDisposable _canExecuteSubscription;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;

        protected UIControl Control => base.Target as UIControl;

        public MvxUIControlTouchUpInsideTargetBinding(UIControl control)
            : base(control)
        {
            if (control == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UIControl is null in MvxUIControlTouchUpInsideTargetBinding");
            }
            else
            {
                control.TouchUpInside += ControlOnTouchUpInside;
            }

            _canExecuteEventHandler = new EventHandler<EventArgs>(this.OnCanExecuteChanged);
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

        public override System.Type TargetType => typeof(ICommand);

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
                    view.TouchUpInside -= ControlOnTouchUpInside;
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