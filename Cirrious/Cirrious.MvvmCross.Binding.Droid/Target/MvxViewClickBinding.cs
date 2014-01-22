// MvxViewClickBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;
using Android.Views;
using Cirrious.CrossCore.WeakSubscription;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxViewClickBinding
        : MvxAndroidTargetBinding
    {
        private ICommand _command;
        private IDisposable _canExecuteSubscription;

        protected View View
        {
            get { return (View)Target; }
        }

        public MvxViewClickBinding(View view)
            : base(view)
        {
            view.Click += ViewOnClick;
        }

        private void ViewOnClick(object sender, EventArgs args)
        {
            if (_command == null)
                return;

            if (!_command.CanExecute(null))
                return;

            _command.Execute(null);
        }

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
                _canExecuteSubscription = _command.WeakSubscribe(OnCanExecuteChanged);
            }
            RefreshEnabledState();
        }

        private void RefreshEnabledState()
        {
            var view = View;
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

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }

        public override Type TargetType
        {
            get { return typeof(ICommand); }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var view = View;
                if (view != null)
                {
                    view.LongClick -= ViewOnClick;
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