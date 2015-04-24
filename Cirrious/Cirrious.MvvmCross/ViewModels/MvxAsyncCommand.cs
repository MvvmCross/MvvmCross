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
        private CancellationTokenSource _cts;
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

        protected CancellationToken CancelToken 
        {
            get 
            {
                return _cts.Token; 
            }
        }

        protected abstract bool DoCanExecute(object parameter);
        protected abstract Task DoExecuteAsync(object parameter);

        public void Cancel()
        {
            lock (_syncRoot)
            {
                if (_cts == null)
                {
                    Mvx.Trace(MvxTraceLevel.Warning, "MvxAsyncCommand : Attempt to cancel a task that is not running");
                }
                else
                {
                    _cts.Cancel();
                }
            }
        }

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
                await ExecuteConcurrentAsync(parameter);
            }
        }

        private async Task ExecuteConcurrentAsync(object parameter)
        {
            bool started = false;
            try
            {
                lock (_syncRoot)
                {
                    if (_concurrentExecutions == 0)
                    {
                        InitCancellationTokenSource();
                    }
                    else if (!_allowConcurrentExecutions)
                    {
                        Mvx.Trace(MvxTraceLevel.Diagnostic, "MvxAsyncCommand : execute ignored, already running.");
                        return;
                    }
                    _concurrentExecutions++;
                    started = true;
                }
                if (!_allowConcurrentExecutions)
                {
                    RaiseCanExecuteChanged();
                }
                await ExecuteWithErrorHandlingAsync(parameter);
            }
            finally
            {
                if (started)
                {
                    lock (_syncRoot)
                    {
                        _concurrentExecutions--;
                        if (_concurrentExecutions == 0)
                        {
                            ClearCancellationTokenSource();
                        }
                    }
                    if (!_allowConcurrentExecutions)
                    {
                        RaiseCanExecuteChanged();
                    }
                }
            }
        }
        
        private async Task ExecuteWithErrorHandlingAsync(object parameter)
        {
            try
            {
                LastError = null;
                if (!CancelToken.IsCancellationRequested)
                {
                    await DoExecuteAsync(parameter);
                }
            }
            catch (Exception e)
            {
                Mvx.Trace(MvxTraceLevel.Error, "MvxAsyncCommand : exception executing task : ", e);
                LastError = e;
            }
        }

        private void ClearCancellationTokenSource()
        {
            if (_cts == null)
            {
                Mvx.Error("MvxAsyncCommand : Unexpected ClearCancellationTokenSource, no token available!");
            }
            else
            {
                _cts.Dispose();
                _cts = null;
            }
        }

        private void InitCancellationTokenSource()
        {
            if (_cts != null)
            {
                Mvx.Error("MvxAsyncCommand : Unexpected InitCancellationTokenSource, a token is already available!");
            }
            _cts = new CancellationTokenSource();
        }
    }

    public class MvxAsyncCommand
        : MvxAsyncCommandBase
        , IMvxCommand
    {
        private readonly Func<CancellationToken, Task> _execute;
        private readonly Func<bool> _canExecute;

        public MvxAsyncCommand(Func<Task> execute, Func<bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = (cancellationToken) => execute();
            _canExecute = canExecute;
        }

        public MvxAsyncCommand(Func<CancellationToken, Task> execute, Func<bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        protected override bool DoCanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        protected override Task DoExecuteAsync(object parameter)
        {
            return _execute(CancelToken);
        }

        public static MvxAsyncCommand<T> CreateCommand<T>(Func<T, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
        {
            return new MvxAsyncCommand<T>(execute, canExecute, allowConcurrentExecutions);
        }

        public static MvxAsyncCommand<T> CreateCommand<T>(Func<T, CancellationToken, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
        {
            return new MvxAsyncCommand<T>(execute, canExecute, allowConcurrentExecutions);
        }
    }

    public class MvxAsyncCommand<T>
        : MvxAsyncCommandBase
        , IMvxCommand
    {
        private readonly Func<T, CancellationToken, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        public MvxAsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = (p, c) => execute(p);
            _canExecute = canExecute;
        }

        public MvxAsyncCommand(Func<T, CancellationToken, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        protected override bool DoCanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)typeof(T).MakeSafeValueCore(parameter));
        }

        protected override Task DoExecuteAsync(object parameter)
        {
            return _execute((T)typeof(T).MakeSafeValueCore(parameter), CancelToken);
        }
    }
}