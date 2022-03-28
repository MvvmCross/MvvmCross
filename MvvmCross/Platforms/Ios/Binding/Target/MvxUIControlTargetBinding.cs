// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Target
{
    public class MvxUIControlTargetBinding : MvxConvertingTargetBinding
    {
        private ICommand _command;
        private IDisposable _canExecuteSubscription;
        private IDisposable _controlEventSubscription;

        private readonly string _controlEvent;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;

        protected UIControl Control => Target as UIControl;

        public MvxUIControlTargetBinding(UIControl control, string controlEvent)
            : base(control)
        {
            _controlEvent = controlEvent;

            if (control == null)
            {
                MvxBindingLog.Error("Error - UIControl is null in MvxUIControlTargetBinding");
            }
            else
            {
                AddHandler(control);
            }

            _canExecuteEventHandler = new EventHandler<EventArgs>(OnCanExecuteChanged);
        }

        private void ControlEvent(object sender, EventArgs eventArgs)
        {
            if (_command == null) return;

            if (!_command.CanExecute(null)) return;

            _command.Execute(null);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetValueType => typeof(ICommand);

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

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                RemoveHandler();
                _canExecuteSubscription?.Dispose();
                _canExecuteSubscription = null;
            }

            base.Dispose(isDisposing);
        }

        private void RefreshEnabledState()
        {
            var view = Control;
            if (view == null) return;

            view.Enabled = _command?.CanExecute(null) ?? false; ;
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            RefreshEnabledState();
        }

        private void AddHandler(UIControl control)
        {
            switch (_controlEvent)
            {
                case MvxIosPropertyBinding.UIControl_TouchDown:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.TouchDown), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_TouchDownRepeat:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.TouchDownRepeat), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_TouchDragInside:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.TouchDragInside), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_TouchUpInside:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.TouchUpInside), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_ValueChanged:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.ValueChanged), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_PrimaryActionTriggered:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.PrimaryActionTriggered), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_EditingDidBegin:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.EditingDidBegin), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_EditingChanged:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.EditingChanged), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_EditingDidEnd:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.EditingDidEnd), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_EditingDidEndOnExit:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.EditingDidEndOnExit), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_AllTouchEvents:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.AllTouchEvents), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_AllEditingEvents:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.AllEditingEvents), ControlEvent);
                    break;
                case MvxIosPropertyBinding.UIControl_AllEvents:
                    _controlEventSubscription = control.WeakSubscribe(nameof(control.AllEvents), ControlEvent);
                    break;
                default:
                    MvxBindingLog.Error("Error - Invalid controlEvent in MvxUIControlTargetBinding");
                    break;
            }
        }

        private void RemoveHandler()
        {
            switch (_controlEvent)
            {
                case MvxIosPropertyBinding.UIControl_TouchDown:
                case MvxIosPropertyBinding.UIControl_TouchDownRepeat:
                case MvxIosPropertyBinding.UIControl_TouchDragInside:
                case MvxIosPropertyBinding.UIControl_TouchUpInside:
                case MvxIosPropertyBinding.UIControl_ValueChanged:
                case MvxIosPropertyBinding.UIControl_PrimaryActionTriggered:
                case MvxIosPropertyBinding.UIControl_EditingDidBegin:
                case MvxIosPropertyBinding.UIControl_EditingChanged:
                case MvxIosPropertyBinding.UIControl_EditingDidEnd:
                case MvxIosPropertyBinding.UIControl_EditingDidEndOnExit:
                case MvxIosPropertyBinding.UIControl_AllTouchEvents:
                case MvxIosPropertyBinding.UIControl_AllEditingEvents:
                case MvxIosPropertyBinding.UIControl_AllEvents:
                    _controlEventSubscription?.Dispose();
                    break;
                default:
                    MvxBindingLog.Error("Error - Invalid controlEvent in MvxUIControlTargetBinding");
                    break;
            }
        }
    }
}
