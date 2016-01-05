// MvxViewClickBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
    using System;
    using System.Windows.Input;

    using Android.Views;

    using MvvmCross.Platform.WeakSubscription;

    public class MvxViewClickBinding
        : MvxAndroidTargetBinding
    {
        private ICommand _command;
        private IDisposable _canExecuteSubscription;
        private readonly EventHandler<EventArgs> _canExecuteEventHandler;

        protected View View => (View)Target;

        public MvxViewClickBinding(View view)
            : base(view)
        {
            this._canExecuteEventHandler = new EventHandler<EventArgs>(this.OnCanExecuteChanged);
            view.Click += this.ViewOnClick;
        }

        private void ViewOnClick(object sender, EventArgs args)
        {
            if (this._command == null)
                return;

            if (!this._command.CanExecute(null))
                return;

            this._command.Execute(null);
        }

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
            var view = this.View;
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

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(ICommand);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var view = this.View;
                if (view != null)
                {
                    view.Click -= this.ViewOnClick;
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