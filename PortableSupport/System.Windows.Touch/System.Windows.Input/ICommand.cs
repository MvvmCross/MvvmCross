// ICommand.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


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