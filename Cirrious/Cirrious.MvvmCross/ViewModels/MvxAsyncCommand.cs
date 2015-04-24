using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.ExtensionMethods;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.ViewModels
{
    public abstract class MvxAsyncCommandBase
        : MvxCommandBase
    {
        private readonly object _syncRoot = new object();
        private readonly bool _allowConcurrentExecutions;
        private int _concurrentExecutions;

        protected MvxAsyncCommandBase(bool allowConcurrentExecutions = false)
        {
            _allowConcurrentExecutions = allowConcurrentExecutions;
        }

        public bool IsRunning
        {
            get { return _concurrentExecutions > 0; }
        }

        public Exception LastError { get; private set; }

        protected abstract bool DoCanExecute(object parameter);
        protected abstract Task DoExecuteAsync(object parameter);

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public bool CanExecute(object parameter)
        {
            if (!_allowConcurrentExecutions && IsRunning)
                return false;
            else
                return DoCanExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public async void Execute()
        {
            await ExecuteAsync(null);
        }
        
        public async Task ExecuteAsync(object parameter)
        {
            if (DoCanExecute(parameter))
            {
                if (_allowConcurrentExecutions)
                {
                    await ExecuteConcurrentAsync(parameter);
                }
                else
                {
                    await ExecuteNoConcurrentAsync(parameter);
                }
            }
        }

        private async Task ExecuteConcurrentAsync(object parameter)
        {
            int concurrentExecutions = -1;
            try
            {
                concurrentExecutions = Interlocked.Increment(ref _concurrentExecutions);
                await ExecuteWithErrorHandlingAsync(parameter);
            }
            finally
            {
                //Only if increment was successful
                if (concurrentExecutions > 0)
                {
                    Interlocked.Decrement(ref _concurrentExecutions);
                }
            }
        }

        private async Task ExecuteNoConcurrentAsync(object parameter)
        {
            int executionsBefore = -1;
            try
            {
                executionsBefore = Interlocked.CompareExchange(ref _concurrentExecutions, 1, 0);
                if (executionsBefore != 0)
                {
                    Mvx.Trace(MvxTraceLevel.Diagnostic, "MvxAsyncCommand : execute ignored, already running.");
                    return;
                }
                RaiseCanExecuteChanged();
                await ExecuteWithErrorHandlingAsync(parameter);
            }
            finally
            {
                if (executionsBefore == 0)
                {
                    Interlocked.Exchange(ref _concurrentExecutions, 0);
                    RaiseCanExecuteChanged();
                }
            }
        }

        private async Task ExecuteWithErrorHandlingAsync(object parameter)
        {
            try
            {
                LastError = null;
                await DoExecuteAsync(parameter);
            }
            catch (Exception e)
            {
                Mvx.Trace(MvxTraceLevel.Error, "MvxAsyncCommand : exception executing task : ", e);
                LastError = e;
            }
        }
    }

    public class MvxAsyncCommand
        : MvxAsyncCommandBase
        , IMvxCommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public MvxAsyncCommand(Func<Task> execute, Func<bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        protected override bool DoCanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        protected override Task DoExecuteAsync(object parameter)
        {
            return _execute();
        }

        public static MvxAsyncCommand<T> CreateCommand<T>(Func<T, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
        {
            return new MvxAsyncCommand<T>(execute, canExecute, allowConcurrentExecutions);
        }
    }

    public class MvxAsyncCommand<T>
        : MvxAsyncCommandBase
        , IMvxCommand
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        public MvxAsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        protected override bool DoCanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)typeof(T).MakeSafeValueCore(parameter));
        }

        protected override Task DoExecuteAsync(object parameter)
        {
            return _execute((T)typeof(T).MakeSafeValueCore(parameter));
        }
    }
}
