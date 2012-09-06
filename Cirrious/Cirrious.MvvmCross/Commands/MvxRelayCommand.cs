#region Copyright
// <copyright file="MvxRelayCommand.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Commands
{
    public class MvxRelayCommand
        : MvxMainThreadDispatchingObject 
        , IMvxCommand 
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

        #region IMvxCommand Members

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
            InvokeOnMainThread(() =>
                                {
                                    if (CanExecute(parameter))
                                    {
                                        _execute();
                                    }
                                });
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

#warning Why is this not derived from MvxMainThreadDispatchingObject? 
    public class MvxRelayCommand<T> 
		: IMvxCommand
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

        #region IMvxCommand Members

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