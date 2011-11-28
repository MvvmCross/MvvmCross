// TODO - need to add credit to MvvmLight and to Josh Smith (http://joshsmithonwpf.wordpress.com) - need to credit MIT
using System;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.MvvmCross.Commands
{
    public class MvxRelayCommand : IMvxCommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public MvxRelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public MvxRelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute();
            }
        }
    }

//#if WINDOWS_PHONE
//    public class MvxRelayCommand<T> : System.Windows.Input.ICommand
//#else
    public class MvxRelayCommand<T> : IMvxCommand
//#endif
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public MvxRelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public MvxRelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _execute((T)parameter);
            }
        }
    }
}
