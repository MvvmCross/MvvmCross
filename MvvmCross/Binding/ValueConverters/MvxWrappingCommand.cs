// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Windows.Input;
using MvvmCross.Logging;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Binding.ValueConverters
{
    public class MvxWrappingCommand
        : ICommand
    {
        private static readonly EventInfo CanExecuteChangedEventInfo = typeof(ICommand).GetEvent("CanExecuteChanged");

        private readonly ICommand _wrapped;
        private readonly object _commandParameterOverride;
        private readonly IDisposable _canChangedEventSubscription;

        public MvxWrappingCommand(ICommand wrapped, object commandParameterOverride)
        {
            _wrapped = wrapped;
            _commandParameterOverride = commandParameterOverride;

            if (_wrapped != null)
            {
                _canChangedEventSubscription = CanExecuteChangedEventInfo.WeakSubscribe(_wrapped, WrappedOnCanExecuteChanged);
            }
        }

        // Note - this is public because we use it in weak referenced situations
        public void WrappedOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            CanExecuteChanged?.Invoke(this, eventArgs);
        }

        public bool CanExecute(object parameter = null)
        {
            if (_wrapped == null)
                return false;

            if (parameter != null)
                MvxLog.Instance.Warn("Non-null parameter will be ignored in MvxWrappingCommand.CanExecute");

            return _wrapped.CanExecute(_commandParameterOverride);
        }

        public void Execute(object parameter = null)
        {
            if (_wrapped == null)
                return;

            if (parameter != null)
                MvxLog.Instance.Warn("Non-null parameter overridden in MvxWrappingCommand");
            _wrapped.Execute(_commandParameterOverride);
        }

        public event EventHandler CanExecuteChanged;
    }
}