// MvxViewLongClickBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using Android.Views;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxViewLongClickBinding
        : MvxConvertingTargetBinding
    {
        private ICommand _command;
        private IDisposable _subscription;

        protected View View => (View)Target;

        public MvxViewLongClickBinding(View view)
            : base(view)
        {
            _subscription = view.WeakSubscribe<View, View.LongClickEventArgs>(nameof(view.LongClick), ViewOnLongClick);
        }

        private void ViewOnLongClick(object sender, View.LongClickEventArgs longClickEventArgs)
        {
            if (_command == null)
                return;

            if (!_command.CanExecute(null))
                return;

            _command.Execute(null);
        }

        protected override void SetValueImpl(object target, object value)
        {
            _command = value as ICommand;
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(ICommand);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;

                _command = null;
            }
            base.Dispose(isDisposing);
        }
    }
}