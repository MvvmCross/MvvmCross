#region Copyright
// <copyright file="MvxCommandConverter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.MvvmCross.WindowsPhone.Platform.Converters
{
    public class MvxCommandConverter
        : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toBeWrapped = value as IMvxCommand;
            if (toBeWrapped == null)
            {
                throw new ArgumentException("value must support IMvxCommand", "value");
            }

            if (toBeWrapped.NativeCommand == null)
            {
                toBeWrapped.NativeCommand = new Wrapper(toBeWrapped);
            }

            if (!(toBeWrapped.NativeCommand is Wrapper))
            {
                throw new MvxException("Unexpected Native Wrapper");
            }

            return toBeWrapped.NativeCommand;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        private class Wrapper : ICommand
        {
            private readonly IMvxCommand _wrapped;

            public Wrapper(IMvxCommand wrapped)
            {
                _wrapped = wrapped;
            }

            #region Implementation of ICommand

            public bool CanExecute(object parameter)
            {
                return _wrapped.CanExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _wrapped.Execute(parameter);
            }

            private int _listenerCount = 0;
            private event EventHandler _canExecuteChanged;

            public event EventHandler CanExecuteChanged
            {
                add
                {
                    _canExecuteChanged += value;
                    _listenerCount++;
                    if (_listenerCount == 1)
                    {
                        _canExecuteChanged += OnCanExecuteChanged;
                    }
                }
                remove
                {
                    _canExecuteChanged -= value;
                    _listenerCount--;
                    if (_listenerCount == 0)
                    {
                        _canExecuteChanged -= OnCanExecuteChanged;
                    }
                }
            }

            private void OnCanExecuteChanged(object sender, EventArgs eventArgs)
            {
                var handler = _canExecuteChanged;
                if (handler != null)
                {
                    handler(this, eventArgs);
                }
            }

            #endregion
        }
    }
}