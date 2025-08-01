// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Base;

namespace MvvmCross.Commands
{
    public interface IMvxCommandHelper
    {
        event EventHandler? CanExecuteChanged;

        void RaiseCanExecuteChanged(object sender);
    }

    public class MvxStrongCommandHelper
        : IMvxCommandHelper
    {
        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged(object sender)
        {
            CanExecuteChanged?.Invoke(sender, EventArgs.Empty);
        }
    }

    public class MvxWeakCommandHelper
        : IMvxCommandHelper
    {
        private readonly List<WeakReference> _eventHandlers = [];
        private readonly object _syncRoot = new();

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                lock (_syncRoot)
                {
                    _eventHandlers.Add(new WeakReference(value));
                }
            }
            remove
            {
                lock (_syncRoot)
                {
                    foreach (var thing in _eventHandlers)
                    {
                        var target = thing.Target;
                        if (target != null && (EventHandler)target == value)
                        {
                            _eventHandlers.Remove(thing);
                            break;
                        }
                    }
                }
            }
        }

        private IEnumerable<EventHandler> SafeCopyEventHandlerList()
        {
            lock (_syncRoot)
            {
                var toReturn = new List<EventHandler>();
                var deadEntries = new List<WeakReference>();

                foreach (var thing in _eventHandlers)
                {
                    if (!thing.IsAlive)
                    {
                        deadEntries.Add(thing);
                        continue;
                    }

                    if (thing.Target is EventHandler eventHandler)
                    {
                        toReturn.Add(eventHandler);
                    }
                }

                foreach (var weakReference in deadEntries)
                {
                    _eventHandlers.Remove(weakReference);
                }

                return toReturn;
            }
        }

        public void RaiseCanExecuteChanged(object sender)
        {
            var list = SafeCopyEventHandlerList();
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

        protected MvxCommandBase()
        {
            if (Mvx.IoCProvider?.TryResolve(out IMvxCommandHelper? commandHelper) == true && commandHelper != null)
            {
                _commandHelper = commandHelper;
            }
            else
            {
                // fallback on MvxWeakCommandHelper if no IoC has been set up
                _commandHelper = new MvxWeakCommandHelper();
            }

            // default to true if no Singleton Cache has been set up
            var alwaysOnUIThread =
                MvxSingletonCache.Instance?.Settings?.AlwaysRaiseInpcOnUserInterfaceThread ?? true;
            ShouldAlwaysRaiseCECOnUserInterfaceThread = alwaysOnUIThread;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => _commandHelper.CanExecuteChanged += value;
            remove => _commandHelper.CanExecuteChanged -= value;
        }

        public bool ShouldAlwaysRaiseCECOnUserInterfaceThread { get; set; }

        public void RaiseCanExecuteChanged()
        {
            if (ShouldAlwaysRaiseCECOnUserInterfaceThread)
            {
                InvokeOnMainThread(() => _commandHelper.RaiseCanExecuteChanged(this));
            }
            else
            {
                _commandHelper.RaiseCanExecuteChanged(this);
            }
        }
    }

    public class MvxCommand
        : MvxCommandBase
        , IMvxCommand
    {
        private readonly Func<bool>? _canExecute;
        private readonly Action _execute;

        public MvxCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
            => _canExecute == null || _canExecute();

        public bool CanExecute()
            => CanExecute(null);

        public void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }

        public void Execute()
            => Execute(null);
    }

    public class MvxCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>
        : MvxCommandBase
        , IMvxCommand, IMvxCommand<T>
    {
        private readonly Func<T?, bool>? _canExecute;
        private readonly Action<T?> _execute;

        public MvxCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
            => _canExecute == null || _canExecute((T?)typeof(T).MakeSafeValueCore(parameter));

        public bool CanExecute()
            => CanExecute(default);

        public bool CanExecute(T? parameter)
            => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter)
        {
            if (!CanExecute(parameter)) return;

            _execute((T?)typeof(T).MakeSafeValueCore(parameter));
        }

        public void Execute()
            => Execute(default);

        public void Execute(T? parameter)
        {
            if (!CanExecute(parameter)) return;

            _execute(parameter);
        }
    }
}
