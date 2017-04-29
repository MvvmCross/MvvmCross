// IMvxCommand.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Windows.Input;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxCommand
        : ICommand
    {
        void RaiseCanExecuteChanged();

        void Execute();

        bool CanExecute();
    }
}