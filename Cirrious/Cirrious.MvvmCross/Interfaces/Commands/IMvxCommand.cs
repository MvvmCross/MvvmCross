#region Copyright
// <copyright file="IMvxCommand.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Windows.Input;

namespace Cirrious.MvvmCross.Interfaces.Commands
{
#if WINDOWS_PHONE
    public interface IMvxCommand : ICommand
    {
        
    }
#else
    public interface IMvxCommand
    {
        bool CanExecute(object parameter);
        void Execute(object parameter);
        event EventHandler CanExecuteChanged;
    }
#endif
}