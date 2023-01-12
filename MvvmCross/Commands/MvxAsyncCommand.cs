// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Logging;

namespace MvvmCross.Commands
{
#nullable enable
    public abstract class MvxAsyncCommandBase
        : MvxCommandBase
    {
        private readonly object _syncRoot = new object();
        private readonly bool _allowConcurrentExecutions;
        private CancellationTokenSource? _cts;
        private int _concurrentExecutions;

        protected MvxAsyncCommandBase(bool allowConcurrentExecutions = false)
        {
            _allowConcurrentExecutions = allowConcurrentExecutions;
        }

        public bool IsRunning => _concurrentExecutions > 0;

        protected CancellationToken CancelToken => _cts?.Token ?? CancellationToken.None;

        protected abstract bool CanExecuteImpl(object? parameter);

        protected abstract Task ExecuteAsyncImpl(object? parameter);

        public void Cancel()
        {
            lock (_syncRoot)
            {
                if (_cts == null)
                {
                    MvxLogHost.Default?.Log(LogLevel.Warning, "MvxAsyncCommand : Attempt to cancel a task that is not running");
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

        public bool CanExecute(object? parameter)
        {
            if (!_allowConcurrentExecutions && IsRunning)
                return false;
            else
                return CanExecuteImpl(parameter);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("AsyncUsage", "AsyncFixer03:Fire-and-forget async-void methods or delegates", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "<Pending>")]
        public async void Execute(object? parameter)
        {
            try
            {
                await ExecuteAsync(parameter, true).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                MvxLogHost.Default?.Log(LogLevel.Error, e, "MvxAsyncCommand : exception executing task");
                throw;
            }
        }

        public void Execute()
        {
            Execute(null);
        }

        protected async Task ExecuteAsync(object? parameter, bool hideCanceledException)
        {
            if (CanExecuteImpl(parameter))
            {
                await ExecuteConcurrentAsync(parameter, hideCanceledException).ConfigureAwait(false);
            }
        }

        private async Task ExecuteConcurrentAsync(object? parameter, bool hideCanceledException)
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
                        MvxLogHost.Default?.Log(LogLevel.Information, "MvxAsyncCommand : execute ignored, already running.");
                        return;
                    }
                    _concurrentExecutions++;
                    started = true;
                }
                if (!_allowConcurrentExecutions)
                {
                    RaiseCanExecuteChanged();
                }
                if (!CancelToken.IsCancellationRequested)
                {
                    try
                    {
                        // With configure await false, the CanExecuteChanged raised in finally clause might run in another thread.
                        // This should not be an issue as long as ShouldAlwaysRaiseCECOnUserInterfaceThread is true.
                        await ExecuteAsyncImpl(parameter).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException e)
                    {
                        MvxLogHost.Default?.Log(LogLevel.Trace, "MvxAsyncCommand : OperationCanceledException");
                        //Rethrow if the exception does not come from the current cancellation token
                        if (!hideCanceledException || e.CancellationToken != CancelToken)
                        {
                            throw;
                        }
                    }
                }
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

        private void ClearCancellationTokenSource()
        {
            if (_cts == null)
            {
                MvxLogHost.Default?.Log(LogLevel.Error, "MvxAsyncCommand : Unexpected ClearCancellationTokenSource, no token available!");
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
                MvxLogHost.Default?.Log(LogLevel.Error, "MvxAsyncCommand : Unexpected InitCancellationTokenSource, a token is already available!");
            }
            _cts = new CancellationTokenSource();
        }
    }

    public class MvxAsyncCommand
        : MvxAsyncCommandBase
        , IMvxAsyncCommand
    {
        private readonly Func<CancellationToken, Task> _execute;
        private readonly Func<bool>? _canExecute;

        public MvxAsyncCommand(Func<Task> execute, Func<bool>? canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = (_) => execute();
            _canExecute = canExecute;
        }

        public MvxAsyncCommand(Func<CancellationToken, Task> execute, Func<bool>? canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        protected override bool CanExecuteImpl(object? parameter)
        {
            return _canExecute == null || _canExecute();
        }

        protected override Task ExecuteAsyncImpl(object? parameter)
        {
            return _execute(CancelToken);
        }

        public static MvxAsyncCommand<T> CreateCommand<T>(Func<T, Task> execute, Func<T, bool>? canExecute = null, bool allowConcurrentExecutions = false)
        {
            return new MvxAsyncCommand<T>(execute, canExecute, allowConcurrentExecutions);
        }

        public static MvxAsyncCommand<T> CreateCommand<T>(Func<T, CancellationToken, Task> execute, Func<T, bool>? canExecute = null, bool allowConcurrentExecutions = false)
        {
            return new MvxAsyncCommand<T>(execute, canExecute, allowConcurrentExecutions);
        }

        public Task ExecuteAsync(object? parameter = null)
        {
            return ExecuteAsync(parameter, false);
        }
    }

    public class MvxAsyncCommand<T>
        : MvxAsyncCommandBase, IMvxCommand, IMvxAsyncCommand<T>
    {
        private readonly Func<T, CancellationToken, Task> _execute;
        private readonly Func<T, bool>? _canExecute;

        public MvxAsyncCommand(Func<T, Task> execute, Func<T, bool>? canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = (p, _) => execute(p);
            _canExecute = canExecute;
        }

        public MvxAsyncCommand(Func<T, CancellationToken, Task> execute, Func<T, bool>? canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public Task ExecuteAsync(T parameter)
            => ExecuteAsync(parameter, false);

        public void Execute(T parameter)
            => base.Execute(parameter);

        public bool CanExecute(T parameter)
            => base.CanExecute(parameter);

        protected override bool CanExecuteImpl(object? parameter)
            => _canExecute == null || _canExecute((T)typeof(T).MakeSafeValueCore(parameter));

        protected override Task ExecuteAsyncImpl(object? parameter)
            => _execute((T)typeof(T).MakeSafeValueCore(parameter), CancelToken);
    }
#nullable restore
}
