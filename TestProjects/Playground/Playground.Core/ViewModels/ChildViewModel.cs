using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ChildViewModel : MvxViewModel
    {
        private ICommand _closeCommand;

        private ICommand _showSecondChildCommand;

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new MvxCommand(() => Close(this))); }
        }

        public ICommand ShowSecondChildCommand
        {
            get
            {
                return _showSecondChildCommand ?? (_showSecondChildCommand =
                           new MvxCommand(() => ShowViewModel<SecondChildViewModel>()));
            }
        }
    }
}