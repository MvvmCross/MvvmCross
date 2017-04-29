using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class ModalNavViewModel : MvxViewModel
    {
        private ICommand _closeCommand;

        private ICommand _showChildCommand;

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new MvxCommand(() => Close(this))); }
        }

        public ICommand ShowChildCommand
        {
            get
            {
                return _showChildCommand ?? (_showChildCommand = new MvxCommand(() => ShowViewModel<ChildViewModel>()));
            }
        }
    }
}