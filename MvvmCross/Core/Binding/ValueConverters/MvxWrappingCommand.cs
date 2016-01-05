// MvxWrappingCommand.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.ValueConverters
{
    using System;
    using System.Reflection;
    using System.Windows.Input;

    using MvvmCross.Platform;
    using MvvmCross.Platform.WeakSubscription;

    public class MvxWrappingCommand
        : ICommand
    {
        private static readonly EventInfo CanExecuteChangedEventInfo = typeof(ICommand).GetEvent("CanExecuteChanged");

        private readonly ICommand _wrapped;
        private readonly object _commandParameterOverride;
        private readonly IDisposable _canChangedEventSubscription;

        public MvxWrappingCommand(ICommand wrapped, object commandParameterOverride)
        {
            this._wrapped = wrapped;
            this._commandParameterOverride = commandParameterOverride;

            if (this._wrapped != null)
            {
                this._canChangedEventSubscription = CanExecuteChangedEventInfo.WeakSubscribe(this._wrapped, WrappedOnCanExecuteChanged);
            }
        }

        // Note - this is public because we use it in weak referenced situations
        public void WrappedOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            var handler = this.CanExecuteChanged;
            handler?.Invoke(this, eventArgs);
        }

        public bool CanExecute(object parameter)
        {
            if (this._wrapped == null)
                return false;

            if (parameter != null)
                Mvx.Warning("Non-null parameter will be ignored in MvxWrappingCommand.CanExecute");

            return this._wrapped.CanExecute(this._commandParameterOverride);
        }

        public void Execute(object parameter)
        {
            if (this._wrapped == null)
                return;

            if (parameter != null)
                Mvx.Warning("Non-null parameter overridden in MvxWrappingCommand");
            this._wrapped.Execute(this._commandParameterOverride);
        }

        public event EventHandler CanExecuteChanged;
    }
}