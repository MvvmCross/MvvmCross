using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Tab1ViewModel : MvxViewModel
    {
        private ICommand _openChildCommand;
        public ICommand OpenChildCommand
        {
            get
            {
                return _openChildCommand ?? (_openChildCommand = new MvxCommand(() => ShowViewModel<ChildViewModel>()));
            }
        }

        private ICommand _openModalCommand;
        public ICommand OpenModalCommand
        {
            get
            {
                return _openModalCommand ?? (_openModalCommand = new MvxCommand(() => ShowViewModel<ModalViewModel>()));
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

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new MvxCommand(() => Close(this)));
            }
        }
    }
}
