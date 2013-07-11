// MvxWrappingCommand.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.CrossCore.WeakSubscription;

namespace Cirrious.MvvmCross.Binding.ValueConverters
{
    public class MvxWrappingCommand
        : ICommand
    {
        private static readonly EventInfo CanExecuteChangedEventInfo = typeof (ICommand).GetEvent("CanExecuteChanged");

        private readonly ICommand _wrapped;
        private readonly object _commandParameterOverride;

        public MvxWrappingCommand(ICommand wrapped, object commandParameterOverride)
        {
            _wrapped = wrapped;
            _commandParameterOverride = commandParameterOverride;

            if (_wrapped != null)
            {
                CanExecuteChangedEventInfo.WeakSubscribe(_wrapped, WrappedOnCanExecuteChanged);
            }
        }

        // Note - this is public because we use it in weak referenced situations
        public void WrappedOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            var handler = CanExecuteChanged;
            if (handler != null)
                handler(this, eventArgs);
        }

        public bool CanExecute(object parameter)
        {
            if (_wrapped == null)
                return false;

            return _wrapped.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (_wrapped == null)
                return;

            if (parameter != null)
                Mvx.Warning("Non-null parameter overridden in MvxWrappingCommand");
            _wrapped.Execute(_commandParameterOverride);
        }

        public event EventHandler CanExecuteChanged;
    }
}