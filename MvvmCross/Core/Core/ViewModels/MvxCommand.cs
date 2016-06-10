// MvxCommand.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.ExtensionMethods;

    public interface IMvxCommandHelper
    {
        event EventHandler CanExecuteChanged;

        void RaiseCanExecuteChanged(object sender);
    }

    public class MvxStrongCommandHelper
        : IMvxCommandHelper
    {
        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged(object sender)
        {
            CanExecuteChanged?.Invoke(sender, EventArgs.Empty);
        }
    }

    public class MvxWeakCommandHelper
        : IMvxCommandHelper
    {
        private readonly List<WeakReference> _eventHandlers = new List<WeakReference>();
        private readonly object _syncRoot = new object();

        public event EventHandler CanExecuteChanged
        {
            add
            {
                lock (this._syncRoot)
                {
                    this._eventHandlers.Add(new WeakReference(value));
                }
            }
            remove
            {
                lock (this._syncRoot)
                {
                    foreach (var thing in this._eventHandlers)
                    {
                        if (thing.IsAlive
                            && ((EventHandler)thing.Target) == value)
                        {
                            this._eventHandlers.Remove(thing);
                            break;
                        }
                    }
                }
            }
        }

        private IEnumerable<EventHandler> SafeCopyEventHandlerList()
        {
            lock (this._syncRoot)
            {
                var toReturn = new List<EventHandler>();
                var deadEntries = new List<WeakReference>();

                foreach (var thing in this._eventHandlers)
                {
                    if (!thing.IsAlive)
                    {
                        deadEntries.Add(thing);
                        continue;
                    }
                    var eventHandler = (EventHandler)thing.Target;
                    if (eventHandler != null)
                    {
                        toReturn.Add(eventHandler);
                    }
                }

                foreach (var weakReference in deadEntries)
                {
                    this._eventHandlers.Remove(weakReference);
                }

                return toReturn;
            }
        }

        public void RaiseCanExecuteChanged(object sender)
        {
            var list = this.SafeCopyEventHandlerList();
            foreach (var eventHandler in list)
            {
                eventHandler(sender, EventArgs.Empty);
            }
        }
    }

    public class MvxCommandBase
        : MvxMainThreadDispatchingObject
    {
        private readonly IMvxCommandHelper _commandHelper;

        public MvxCommandBase()
        {
            if (!Mvx.TryResolve<IMvxCommandHelper>(out this._commandHelper))
                this._commandHelper = new MvxWeakCommandHelper();

            var alwaysOnUIThread = (MvxSingletonCache.Instance == null) || MvxSingletonCache.Instance.Settings.AlwaysRaiseInpcOnUserInterfaceThread;
            this.ShouldAlwaysRaiseCECOnUserInterfaceThread = alwaysOnUIThread;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                this._commandHelper.CanExecuteChanged += value;
            }
            remove
            {
                this._commandHelper.CanExecuteChanged -= value;
            }
        }

        public bool ShouldAlwaysRaiseCECOnUserInterfaceThread { get; set; }

        public void RaiseCanExecuteChanged()
        {
            if (this.ShouldAlwaysRaiseCECOnUserInterfaceThread)
            {
                this.InvokeOnMainThread(() => this._commandHelper.RaiseCanExecuteChanged(this));
            }
            else
            {
                this._commandHelper.RaiseCanExecuteChanged(this);
            }
        }
    }

    public class MvxCommand
        : MvxCommandBase
        , IMvxCommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        public MvxCommand(Action execute)
            : this(execute, null)
        {
        }

        public MvxCommand(Action execute, Func<bool> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute();
        }

        public bool CanExecute()
        {
            return this.CanExecute(null);
        }

        public void Execute(object parameter)
        {
            if (this.CanExecute(parameter))
            {
                this._execute();
            }
        }

        public void Execute()
        {
            this.Execute(null);
        }
    }

    public class MvxCommand<T>
        : MvxCommandBase
        , IMvxCommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;

        public MvxCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public MvxCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute == null || this._canExecute((T)(typeof(T).MakeSafeValueCore(parameter)));
        }

        public bool CanExecute()
        {
            return this.CanExecute(null);
        }

        public void Execute(object parameter)
        {
            if (this.CanExecute(parameter))
            {
                this._execute((T)(typeof(T).MakeSafeValueCore(parameter)));
            }
        }

        public void Execute()
        {
            this.Execute(null);
        }
    }
}