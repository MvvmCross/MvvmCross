using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ChildViewModel : MvxViewModel
    {
        public ChildViewModel()
        {
        }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new MvxCommand(() => Close(this)));
            }
        }

        private ICommand _showSecondChildCommand;
        public ICommand ShowSecondChildCommand
        {
            get
            {
                return _showSecondChildCommand ?? (_showSecondChildCommand = new MvxCommand(() => ShowViewModel<SecondChildViewModel>()));
            }
        }
    }
}
