// MvxViewLongClickBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
    using System;
    using System.Windows.Input;

    using Android.Views;

    public class MvxViewLongClickBinding
        : MvxAndroidTargetBinding
    {
        private ICommand _command;

        protected View View => (View)Target;

        public MvxViewLongClickBinding(View view)
            : base(view)
        {
            view.LongClick += this.ViewOnLongClick;
        }

        private void ViewOnLongClick(object sender, View.LongClickEventArgs longClickEventArgs)
        {
            if (this._command == null)
                return;

            if (!this._command.CanExecute(null))
                return;

            this._command.Execute(null);
        }

        protected override void SetValueImpl(object target, object value)
        {
            this._command = value as ICommand;
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
                    view.LongClick -= this.ViewOnLongClick;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}