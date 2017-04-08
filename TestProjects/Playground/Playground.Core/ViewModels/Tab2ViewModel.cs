using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Tab2ViewModel : MvxViewModel
    {
        private ICommand _showRootViewModelCommand;
        public ICommand ShowRootViewModelCommand
        {
            get
            {
                return _showRootViewModelCommand ?? (_showRootViewModelCommand = new MvxCommand(() => ShowViewModel<RootViewModel>()));
            }
        }

        private ICommand _closeViewModelCommand;
        public ICommand CloseViewModelCommand
        {
            get
            {
                return _closeViewModelCommand ?? (_closeViewModelCommand = new MvxCommand(() => Close(this)));
            }
        }
    }
}
