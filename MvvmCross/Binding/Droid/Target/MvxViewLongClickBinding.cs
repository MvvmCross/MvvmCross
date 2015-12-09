// MvxViewLongClickBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using System;
using System.Windows.Input;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxViewLongClickBinding
        : MvxAndroidTargetBinding
    {
        private ICommand _command;

        protected View View => (View)Target;

        public MvxViewLongClickBinding(View view)
            : base(view)
        {
            view.LongClick += ViewOnLongClick;
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
                var view = View;
                if (view != null)
                {
                    view.LongClick -= ViewOnLongClick;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}