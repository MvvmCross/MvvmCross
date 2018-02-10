// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MvvmCross.ViewModels
{
    /// <summary>
    /// Watches a task and raises property-changed notifications when the task completes.
    /// </summary>
    public sealed class MvxNotifyTask : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a task notifier watching the specified task.
        /// </summary>
        /// <param name="task">The task to watch.</param>
        private MvxNotifyTask(Task task)
        {
            Task = task;
            TaskCompleted = MonitorTaskAsync(task);
        }

        private async Task MonitorTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
            finally
            {
                NotifyProperties(task);
            }
        }

        private void NotifyProperties(Task task)
        {
            var propertyChanged = PropertyChanged;
            if(propertyChanged == null)
                return;

            if(task.IsCanceled)
            {
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("Status"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("IsCanceled"));
            }
            else if(task.IsFaulted)
            {
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("Exception"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("InnerException"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("ErrorMessage"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("Status"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("IsFaulted"));
            }
            else
            {
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("Status"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("IsSuccessfullyCompleted"));
            }
            propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("IsCompleted"));
            propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("IsNotCompleted"));
        }

        /// <summary>
        /// Gets the task being watched. This property never changes and is never <c>null</c>.
        /// </summary>
        public Task Task { get; private set; }

        /// <summary>
        /// Gets a task that completes successfully when <see cref="Task"/> completes (successfully, faulted, or canceled). This property never changes and is never <c>null</c>.
        /// </summary>
        public Task TaskCompleted { get; private set; }

        /// <summary>
        /// Gets the current task status. This property raises a notification when the task completes.
        /// </summary>
        public TaskStatus Status { get { return Task.Status; } }

        /// <summary>
        /// Gets whether the task has completed. This property raises a notification when the value changes to <c>true</c>.
        /// </summary>
        public bool IsCompleted { get { return Task.IsCompleted; } }

        /// <summary>
        /// Gets whether the task is busy (not completed). This property raises a notification when the value changes to <c>false</c>.
        /// </summary>
        public bool IsNotCompleted { get { return !Task.IsCompleted; } }

        /// <summary>
        /// Gets whether the task has completed successfully. This property raises a notification when the value changes to <c>true</c>.
        /// </summary>
        public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }

        /// <summary>
        /// Gets whether the task has been canceled. This property raises a notification only if the task is canceled (i.e., if the value changes to <c>true</c>).
        /// </summary>
        public bool IsCanceled { get { return Task.IsCanceled; } }

        /// <summary>
        /// Gets whether the task has faulted. This property raises a notification only if the task faults (i.e., if the value changes to <c>true</c>).
        /// </summary>
        public bool IsFaulted { get { return Task.IsFaulted; } }

        /// <summary>
        /// Gets the wrapped faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public AggregateException Exception { get { return Task.Exception; } }

        /// <summary>
        /// Gets the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public Exception InnerException { get { return (Exception == null) ? null : Exception.InnerException; } }

        /// <summary>
        /// Gets the error message for the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public string ErrorMessage { get { return (InnerException == null) ? null : InnerException.Message; } }

        /// <summary>
        /// Event that notifies listeners of property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new task notifier watching the specified task.
        /// </summary>
        /// <param name="task">The task to watch.</param>
        public static MvxNotifyTask Create(Task task)
        {
            return new MvxNotifyTask(task);
        }

        /// <summary>
        /// Creates a new task notifier watching the specified task.
        /// </summary>
        /// <typeparam name="TResult">The type of the task result.</typeparam>
        /// <param name="task">The task to watch.</param>
        /// <param name="defaultResult">The default "result" value for the task while it is not yet complete.</param>
        public static MvxNotifyTask<TResult> Create<TResult>(Task<TResult> task, TResult defaultResult = default(TResult))
        {
            return new MvxNotifyTask<TResult>(task, defaultResult);
        }

        /// <summary>
        /// Executes the specified asynchronous code and creates a new task notifier watching the returned task.
        /// </summary>
        /// <param name="asyncAction">The asynchronous code to execute.</param>
        public static MvxNotifyTask Create(Func<Task> asyncAction)
        {
            return Create(asyncAction());
        }

        /// <summary>
        /// Executes the specified asynchronous code and creates a new task notifier watching the returned task.
        /// </summary>
        /// <param name="asyncAction">The asynchronous code to execute.</param>
        /// <param name="defaultResult">The default "result" value for the task while it is not yet complete.</param>
        public static MvxNotifyTask<TResult> Create<TResult>(Func<Task<TResult>> asyncAction, TResult defaultResult = default(TResult))
        {
            return Create(asyncAction(), defaultResult);
        }
    }

    /// <summary>
    /// Watches a task and raises property-changed notifications when the task completes.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    public sealed class MvxNotifyTask<TResult> : INotifyPropertyChanged
    {
        /// <summary>
        /// The "result" of the task when it has not yet completed.
        /// </summary>
        private readonly TResult _defaultResult;

        /// <summary>
        /// Initializes a task notifier watching the specified task.
        /// </summary>
        /// <param name="task">The task to watch.</param>
        /// <param name="defaultResult">The value to return from <see cref="Result"/> while the task is not yet complete.</param>
        internal MvxNotifyTask(Task<TResult> task, TResult defaultResult)
        {
            _defaultResult = defaultResult;
            Task = task;
            TaskCompleted = MonitorTaskAsync(task);
        }

        private async Task MonitorTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
            finally
            {
                NotifyProperties(task);
            }
        }

        private void NotifyProperties(Task task)
        {
            var propertyChanged = PropertyChanged;
            if(propertyChanged == null)
                return;

            if(task.IsCanceled)
            {
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("Status"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("IsCanceled"));
            }
            else if(task.IsFaulted)
            {
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("Exception"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("InnerException"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("ErrorMessage"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("Status"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("IsFaulted"));
            }
            else
            {
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("Result"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("Status"));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("IsSuccessfullyCompleted"));
            }
            propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("IsCompleted"));
            propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get("IsNotCompleted"));
        }

        /// <summary>
        /// Gets the task being watched. This property never changes and is never <c>null</c>.
        /// </summary>
        public Task<TResult> Task { get; private set; }

        /// <summary>
        /// Gets a task that completes successfully when <see cref="Task"/> completes (successfully, faulted, or canceled). This property never changes and is never <c>null</c>.
        /// </summary>
        public Task TaskCompleted { get; private set; }

        /// <summary>
        /// Gets the result of the task. Returns the "default result" value specified in the constructor if the task has not yet completed successfully. This property raises a notification when the task completes successfully.
        /// </summary>
        public TResult Result { get { return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : _defaultResult; } }

        /// <summary>
        /// Gets the current task status. This property raises a notification when the task completes.
        /// </summary>
        public TaskStatus Status { get { return Task.Status; } }

        /// <summary>
        /// Gets whether the task has completed. This property raises a notification when the value changes to <c>true</c>.
        /// </summary>
        public bool IsCompleted { get { return Task.IsCompleted; } }

        /// <summary>
        /// Gets whether the task is busy (not completed). This property raises a notification when the value changes to <c>false</c>.
        /// </summary>
        public bool IsNotCompleted { get { return !Task.IsCompleted; } }

        /// <summary>
        /// Gets whether the task has completed successfully. This property raises a notification when the value changes to <c>true</c>.
        /// </summary>
        public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }

        /// <summary>
        /// Gets whether the task has been canceled. This property raises a notification only if the task is canceled (i.e., if the value changes to <c>true</c>).
        /// </summary>
        public bool IsCanceled { get { return Task.IsCanceled; } }

        /// <summary>
        /// Gets whether the task has faulted. This property raises a notification only if the task faults (i.e., if the value changes to <c>true</c>).
        /// </summary>
        public bool IsFaulted { get { return Task.IsFaulted; } }

        /// <summary>
        /// Gets the wrapped faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public AggregateException Exception { get { return Task.Exception; } }

        /// <summary>
        /// Gets the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public Exception InnerException { get { return (Exception == null) ? null : Exception.InnerException; } }

        /// <summary>
        /// Gets the error message for the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public string ErrorMessage { get { return (InnerException == null) ? null : InnerException.Message; } }

        /// <summary>
        /// Event that notifies listeners of property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
