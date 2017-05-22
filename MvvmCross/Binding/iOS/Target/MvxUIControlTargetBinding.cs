// MvxUIControlTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Target
{
    using System;
    using System.Windows.Input;

    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.WeakSubscription;
    using UIKit;

    public class MvxUIControlTargetBinding : MvxConvertingTargetBinding
    {
        private ICommand _command;
        private IDisposable _canExecuteSubscription;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;
		private readonly string _controlEvent;
        protected UIControl Control => base.Target as UIControl;

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

            this._canExecuteEventHandler = new EventHandler<EventArgs>(this.OnCanExecuteChanged);
        }

        private void ControlEvent(object sender, EventArgs eventArgs)
        {
            if (this._command == null)
                return;

            if (!this._command.CanExecute(null))
                return;

            this._command.Execute(null);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override System.Type TargetType => typeof(ICommand);

        protected override void SetValueImpl(object target, object value)
        {
            if (this._canExecuteSubscription != null)
            {
                this._canExecuteSubscription.Dispose();
                this._canExecuteSubscription = null;
            }
            this._command = value as ICommand;
            if (this._command != null)
            {
                this._canExecuteSubscription = this._command.WeakSubscribe(this._canExecuteEventHandler);
            }
            this.RefreshEnabledState();
        }

        private void RefreshEnabledState()
        {
            var view = this.Control;
            if (view == null)
                return;

            var shouldBeEnabled = false;
            if (this._command != null)
            {
                shouldBeEnabled = this._command.CanExecute(null);
            }
            view.Enabled = shouldBeEnabled;
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            this.RefreshEnabledState();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var view = this.Control;
                if (view != null)
                {
					RemoveHandler(view);
                }
                if (this._canExecuteSubscription != null)
                {
                    this._canExecuteSubscription.Dispose();
                    this._canExecuteSubscription = null;
                }
            }
            base.Dispose(isDisposing);
        }

		private void AddHandler(UIControl control)
		{
            switch (_controlEvent)
            {
                case (MvxIosPropertyBinding.UIControl_TouchDown):
                    control.TouchDown += this.ControlEvent;
                    break;
                case (MvxIosPropertyBinding.UIControl_TouchDownRepeat):
                    control.TouchDownRepeat += this.ControlEvent;
                    break;
                case (MvxIosPropertyBinding.UIControl_TouchDragInside):
                    control.TouchDragInside += this.ControlEvent;
                    break;
                case (MvxIosPropertyBinding.UIControl_TouchUpInside):
                    control.TouchUpInside += this.ControlEvent;
                    break;
                case (MvxIosPropertyBinding.UIControl_ValueChanged):
                    control.ValueChanged += this.ControlEvent;
                    break;
                case (MvxIosPropertyBinding.UIControl_PrimaryActionTriggered):
                    control.PrimaryActionTriggered += this.ControlEvent;
                    break;
                case (MvxIosPropertyBinding.UIControl_EditingDidBegin):
                    control.EditingDidBegin += this.ControlEvent;
                    break;
                case (MvxIosPropertyBinding.UIControl_EditingChanged):
                    control.EditingChanged += this.ControlEvent;
                    break;
                case (MvxIosPropertyBinding.UIControl_EditingDidEnd):
                    control.EditingDidEnd += this.ControlEvent;
                    break;
                case (MvxIosPropertyBinding.UIControl_EditingDidEndOnExit):
                    control.EditingDidEndOnExit += this.ControlEvent;
				    break;
                case (MvxIosPropertyBinding.UIControl_AllTouchEvents):
                    control.AllTouchEvents += this.ControlEvent;
                    break;
                case (MvxIosPropertyBinding.UIControl_AllEditingEvents):
                    control.AllEditingEvents += this.ControlEvent;
                    break;
                case (MvxIosPropertyBinding.UIControl_AllEvents):
                    control.AllEvents += this.ControlEvent;
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
				case (MvxIosPropertyBinding.UIControl_TouchDown):
					control.TouchDown -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_TouchDownRepeat):
					control.TouchDownRepeat -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_TouchDragInside):
					control.TouchDragInside -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_TouchUpInside):
					control.TouchUpInside -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_ValueChanged):
					control.ValueChanged -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_PrimaryActionTriggered):
					control.PrimaryActionTriggered -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_EditingDidBegin):
					control.EditingDidBegin -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_EditingChanged):
					control.EditingChanged -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_EditingDidEnd):
					control.EditingDidEnd -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_EditingDidEndOnExit):
					control.EditingDidEndOnExit -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_AllTouchEvents):
					control.AllTouchEvents -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_AllEditingEvents):
					control.AllEditingEvents -= this.ControlEvent;
					break;
				case (MvxIosPropertyBinding.UIControl_AllEvents):
					control.AllEvents -= this.ControlEvent;
					break;
				default:
					MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - Invalid controlEvent in MvxUIControlTargetBinding");
					break;
			}
        }
    }
}