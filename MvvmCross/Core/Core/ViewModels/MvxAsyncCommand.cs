namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MvvmCross.Platform;
    using MvvmCross.Platform.ExtensionMethods;
    using MvvmCross.Platform.Platform;

    public abstract class MvxAsyncCommandBase
        : MvxCommandBase
    {
        private readonly object _syncRoot = new object();
        private readonly bool _allowConcurrentExecutions;
        private CancellationTokenSource _cts;
        private int _concurrentExecutions;

        protected MvxAsyncCommandBase(bool allowConcurrentExecutions = false)
        {
            this._allowConcurrentExecutions = allowConcurrentExecutions;
        }

        public bool IsRunning => this._concurrentExecutions > 0;

        protected CancellationToken CancelToken => this._cts.Token;

        protected abstract bool CanExecuteImpl(object parameter);

        protected abstract Task ExecuteAsyncImpl(object parameter);

        public void Cancel()
        {
            lock (this._syncRoot)
            {
                if (this._cts == null)
                {
                    Mvx.Trace(MvxTraceLevel.Warning, "MvxAsyncCommand : Attempt to cancel a task that is not running");
                }
                else
                {
                    this._cts.Cancel();
                }
            }
        }

        public bool CanExecute()
        {
            return this.CanExecute(null);
        }

        public bool CanExecute(object parameter)
        {
            if (!this._allowConcurrentExecutions && this.IsRunning)
                return false;
            else
                return this.CanExecuteImpl(parameter);
        }

        public async void Execute(object parameter)
        {
            try
            {
                await this.ExecuteAsync(parameter, true).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Mvx.Trace(MvxTraceLevel.Error, "MvxAsyncCommand : exception executing task : ", e);
                throw;
            }
        }

        public void Execute()
        {
            this.Execute(null);
        }
        
        protected async Task ExecuteAsync(object parameter, bool hideCanceledException)
        {
            if (this.CanExecuteImpl(parameter))
            {
                await this.ExecuteConcurrentAsync(parameter, hideCanceledException).ConfigureAwait(false);
            }
        }

        private async Task ExecuteConcurrentAsync(object parameter, bool hideCanceledException)
        {
            bool started = false;
            try
            {
                lock (this._syncRoot)
                {
                    if (this._concurrentExecutions == 0)
                    {
                        this.InitCancellationTokenSource();
                    }
                    else if (!this._allowConcurrentExecutions)
                    {
                        Mvx.Trace(MvxTraceLevel.Diagnostic, "MvxAsyncCommand : execute ignored, already running.");
                        return;
                    }
                    this._concurrentExecutions++;
                    started = true;
                }
                if (!this._allowConcurrentExecutions)
                {
                    this.RaiseCanExecuteChanged();
                }
                if (!this.CancelToken.IsCancellationRequested)
                {
                    try
                    {
                        // With configure await false, the CanExecuteChanged raised in finally clause might run in another thread.
                        // This should not be an issue as long as ShouldAlwaysRaiseCECOnUserInterfaceThread is true.
                        await this.ExecuteAsyncImpl(parameter).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException e)
                    {
                        Mvx.Trace(MvxTraceLevel.Diagnostic, "MvxAsyncCommand : OperationCanceledException");
                        //Rethrow if the exception does not comes from the current cancellation token
                        if (!hideCanceledException || e.CancellationToken != this.CancelToken)
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
                    lock (this._syncRoot)
                    {
                        this._concurrentExecutions--;
                        if (this._concurrentExecutions == 0)
                        {
                            this.ClearCancellationTokenSource();
                        }
                    }
                    if (!this._allowConcurrentExecutions)
                    {
                        this.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        private void ClearCancellationTokenSource()
        {
            if (this._cts == null)
            {
                Mvx.Error("MvxAsyncCommand : Unexpected ClearCancellationTokenSource, no token available!");
            }
            else
            {
                this._cts.Dispose();
                this._cts = null;
            }
        }

        private void InitCancellationTokenSource()
        {
            if (this._cts != null)
            {
                Mvx.Error("MvxAsyncCommand : Unexpected InitCancellationTokenSource, a token is already available!");
            }
            this._cts = new CancellationTokenSource();
        }
    }

    public class MvxAsyncCommand
        : MvxAsyncCommandBase
        , IMvxAsyncCommand
    {
        private readonly Func<CancellationToken, Task> _execute;
        private readonly Func<bool> _canExecute;

        public MvxAsyncCommand(Func<Task> execute, Func<bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            this._execute = (cancellationToken) => execute();
            this._canExecute = canExecute;
        }

        public MvxAsyncCommand(Func<CancellationToken, Task> execute, Func<bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            this._execute = execute;
            this._canExecute = canExecute;
        }

        protected override bool CanExecuteImpl(object parameter)
        {
            return this._canExecute == null || this._canExecute();
        }

        protected override Task ExecuteAsyncImpl(object parameter)
        {
            return this._execute(this.CancelToken);
        }

        public static MvxAsyncCommand<T> CreateCommand<T>(Func<T, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
        {
            return new MvxAsyncCommand<T>(execute, canExecute, allowConcurrentExecutions);
        }

        public static MvxAsyncCommand<T> CreateCommand<T>(Func<T, CancellationToken, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
        {
            return new MvxAsyncCommand<T>(execute, canExecute, allowConcurrentExecutions);
        }

        public async Task ExecuteAsync(object parameter = null)
        {
            await base.ExecuteAsync(parameter, false).ConfigureAwait(false);
        }
    }

    public class MvxAsyncCommand<T>
        : MvxAsyncCommandBase
        , IMvxAsyncCommand
    {
        private readonly Func<T, CancellationToken, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        public MvxAsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            this._execute = (p, c) => execute(p);
            this._canExecute = canExecute;
        }

        public MvxAsyncCommand(Func<T, CancellationToken, Task> execute, Func<T, bool> canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            this._execute = execute;
            this._canExecute = canExecute;
        }

        protected override bool CanExecuteImpl(object parameter)
        {
            return this._canExecute == null || this._canExecute((T)typeof(T).MakeSafeValueCore(parameter));
        }

        protected override Task ExecuteAsyncImpl(object parameter)
        {
            return this._execute((T)typeof(T).MakeSafeValueCore(parameter), this.CancelToken);
        }

        public async Task ExecuteAsync(object parameter)
        {
            await base.ExecuteAsync(parameter, false).ConfigureAwait(false);
        }
    }
}