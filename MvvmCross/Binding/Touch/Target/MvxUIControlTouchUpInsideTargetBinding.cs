// MvxUIControlTouchUpInsideTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Target
{
    using System;
    using System.Windows.Input;

    using MvvmCross.Binding.Bindings.Target;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.WeakSubscription;

    using UIKit;

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
                control.TouchUpInside += this.ControlOnTouchUpInside;
            }

            this._canExecuteEventHandler = new EventHandler<EventArgs>(this.OnCanExecuteChanged);
        }

        private void ControlOnTouchUpInside(object sender, EventArgs eventArgs)
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
                    view.TouchUpInside -= this.ControlOnTouchUpInside;
                }
                if (this._canExecuteSubscription != null)
                {
                    this._canExecuteSubscription.Dispose();
                    this._canExecuteSubscription = null;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}