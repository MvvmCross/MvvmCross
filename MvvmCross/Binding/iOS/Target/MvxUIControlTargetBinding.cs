// MvxUIControlTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUIControlTargetBinding : MvxConvertingTargetBinding
    {
        private ICommand _command;
        private IDisposable _canExecuteSubscription;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;
		private readonly string _controlEvent;
        protected UIControl Control => Target as UIControl;

		public MvxUIControlTargetBinding(UIControl control, string controlEvent)
            : base(control)
        {
			_controlEvent = controlEvent;

            if (control == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UIControl is null in MvxUIControlTargetBinding");
            }
            else
            {
				AddHandler(control);	
            }

            _canExecuteEventHandler = new EventHandler<EventArgs>(OnCanExecuteChanged);
        }

        private void ControlEvent(object sender, EventArgs eventArgs)
        {
            if (_command == null)
                return;

            if (!_command.CanExecute(null))
                return;

            _command.Execute(null);
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
					RemoveHandler(view);
                }
                if (_canExecuteSubscription != null)
                {
                    _canExecuteSubscription.Dispose();
                    _canExecuteSubscription = null;
                }
            }
            base.Dispose(isDisposing);
        }

		private void AddHandler(UIControl control)
		{
            switch (_controlEvent)
            {
                case MvxIosPropertyBinding.UIControl_TouchDown:
                    control.TouchDown += ControlEvent;
                    break;
                case MvxIosPropertyBinding.UIControl_TouchDownRepeat:
                    control.TouchDownRepeat += ControlEvent;
                    break;
                case MvxIosPropertyBinding.UIControl_TouchDragInside:
                    control.TouchDragInside += ControlEvent;
                    break;
                case MvxIosPropertyBinding.UIControl_TouchUpInside:
                    control.TouchUpInside += ControlEvent;
                    break;
                case MvxIosPropertyBinding.UIControl_ValueChanged:
                    control.ValueChanged += ControlEvent;
                    break;
                case MvxIosPropertyBinding.UIControl_PrimaryActionTriggered:
                    control.PrimaryActionTriggered += ControlEvent;
                    break;
                case MvxIosPropertyBinding.UIControl_EditingDidBegin:
                    control.EditingDidBegin += ControlEvent;
                    break;
                case MvxIosPropertyBinding.UIControl_EditingChanged:
                    control.EditingChanged += ControlEvent;
                    break;
                case MvxIosPropertyBinding.UIControl_EditingDidEnd:
                    control.EditingDidEnd += ControlEvent;
                    break;
                case MvxIosPropertyBinding.UIControl_EditingDidEndOnExit:
                    control.EditingDidEndOnExit += ControlEvent;
				    break;
                case MvxIosPropertyBinding.UIControl_AllTouchEvents:
                    control.AllTouchEvents += ControlEvent;
                    break;
                case MvxIosPropertyBinding.UIControl_AllEditingEvents:
                    control.AllEditingEvents += ControlEvent;
                    break;
                case MvxIosPropertyBinding.UIControl_AllEvents:
                    control.AllEvents += ControlEvent;
                    break;
                default:
                    MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - Invalid controlEvent in MvxUIControlTargetBinding");
                    break;
            }
		}

		private void RemoveHandler(UIControl control)
		{
			switch (_controlEvent)
			{
				case MvxIosPropertyBinding.UIControl_TouchDown:
					control.TouchDown -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_TouchDownRepeat:
					control.TouchDownRepeat -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_TouchDragInside:
					control.TouchDragInside -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_TouchUpInside:
					control.TouchUpInside -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_ValueChanged:
					control.ValueChanged -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_PrimaryActionTriggered:
					control.PrimaryActionTriggered -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_EditingDidBegin:
					control.EditingDidBegin -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_EditingChanged:
					control.EditingChanged -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_EditingDidEnd:
					control.EditingDidEnd -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_EditingDidEndOnExit:
					control.EditingDidEndOnExit -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_AllTouchEvents:
					control.AllTouchEvents -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_AllEditingEvents:
					control.AllEditingEvents -= ControlEvent;
					break;
				case MvxIosPropertyBinding.UIControl_AllEvents:
					control.AllEvents -= ControlEvent;
					break;
				default:
					MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - Invalid controlEvent in MvxUIControlTargetBinding");
					break;
			}
        }
    }
}