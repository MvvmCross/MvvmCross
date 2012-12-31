#region Copyright

// <copyright file="ICommand.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion


#if HACK_DO_NOT_FORWARD_ICOMMAND
// ReSharper disable CheckNamespace

namespace System.Windows.Input
// ReSharper restore CheckNamespace
{
    // removed ICommand as latest monotouch versions have System.Windows.Input in them!
    public interface ICommand
    {
        // Events
        event EventHandler CanExecuteChanged;

        // Methods
        bool CanExecute(object parameter);
        void Execute(object parameter);
    }
}

#endif