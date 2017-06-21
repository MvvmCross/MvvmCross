using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ModalViewModel : MvxViewModel
    {
        private ICommand _showTabsCommand;
        public ICommand ShowTabsCommand
        {
            get
            {
                return _showTabsCommand ?? (_showTabsCommand = new MvxCommand(() => ShowViewModel<TabsRootViewModel>()));
            }
        }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new MvxCommand(() => Close(this)));
            }
        }

        private ICommand _openNavModalCommand;
        public ICommand OpenNavModalCommand
        {
            get
            {
                return _openNavModalCommand ?? (_openNavModalCommand = new MvxCommand(() => ShowViewModel<ModalNavViewModel>()));
            }
        }
    }
}
