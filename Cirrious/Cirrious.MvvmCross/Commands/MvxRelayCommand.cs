// MvxRelayCommand.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;

namespace Cirrious.MvvmCross.Commands
{
    public class MvxRelayCommand
        : ICommand
          , IDisposable
    {
        private Func<bool> _canExecute;
        private Action _execute;

        public MvxRelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public MvxRelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

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

        #endregion

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _execute = null;
                _canExecute = null;
            }
        }

        #endregion
    }

    public class MvxRelayCommand<T>
        : ICommand
          , IDisposable
    {
        private Func<T, bool> _canExecute;
        private Action<T> _execute;

        public MvxRelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public MvxRelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

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

        #endregion

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _execute = null;
                _canExecute = null;
            }
        }

        #endregion
    }
}