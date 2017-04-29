using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Tab2ViewModel : MvxViewModel
    {
        private ICommand _closeViewModelCommand;
        private ICommand _showRootViewModelCommand;

        public ICommand ShowRootViewModelCommand
        {
            get
            {
                return _showRootViewModelCommand ?? (_showRootViewModelCommand =
                           new MvxCommand(() => ShowViewModel<RootViewModel>()));
            }
        }

        public ICommand CloseViewModelCommand
        {
            get { return _closeViewModelCommand ?? (_closeViewModelCommand = new MvxCommand(() => Close(this))); }
        }
    }
}