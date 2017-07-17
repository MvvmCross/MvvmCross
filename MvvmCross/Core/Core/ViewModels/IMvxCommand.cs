// IMvxCommand.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Windows.Input;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxCommand : ICommand
    {
        void RaiseCanExecuteChanged();

        void Execute();

        bool CanExecute();
    }

    public interface IMvxCommand<T> : ICommand
    {
        [Obsolete("Use the strongly typed version of Execute instead", true)]
        new void Execute(object parameter);

        [Obsolete("Use the strongly typed version of CanExecute instead", true)]
        new bool CanExecute(object parameter);

        void Execute(T parameter);

        bool CanExecute(T parameter);
    }
}