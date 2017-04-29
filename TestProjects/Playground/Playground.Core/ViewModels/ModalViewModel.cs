using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ModalViewModel : MvxViewModel
    {
        private ICommand _closeCommand;
        private ICommand _showTabsCommand;

        public ICommand ShowTabsCommand
        {
            get
            {
                return _showTabsCommand ?? (_showTabsCommand =
                           new MvxCommand(() => ShowViewModel<TabsRootViewModel>()));
            }
        }

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new MvxCommand(() => Close(this))); }
        }
    }
}