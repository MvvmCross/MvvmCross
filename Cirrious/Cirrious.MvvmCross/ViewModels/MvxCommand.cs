﻿// MvxCommand.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.ViewModels
{
    public class MvxCommandBase
    {
        private readonly List<WeakReference> _eventHandlers = new List<WeakReference>();

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _eventHandlers.Add(new WeakReference(value));
            }
            remove
            {
                foreach (var thing in _eventHandlers)
                {
                    if (thing.IsAlive
                        && ((EventHandler)thing.Target) == value)
                    {
                        _eventHandlers.Remove(thing);
                        break;
                    }
                }
            }
        }

        private List<EventHandler> SafeCopyEventHandlerList()
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
                var eventHandler = (EventHandler) thing.Target;
                if (eventHandler != null)
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

        public void RaiseCanExecuteChanged()
        {
            var list = SafeCopyEventHandlerList();
            foreach (var eventHandler in list)
            {
                eventHandler(this, EventArgs.Empty);
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
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }

        public void Execute()
        {
            Execute(null);
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
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T) parameter);
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute((T) parameter);
            }
        }

        public void Execute()
        {
            Execute(null);
        }
    }
}