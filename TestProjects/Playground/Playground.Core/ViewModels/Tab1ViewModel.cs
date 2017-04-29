using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Tab1ViewModel : MvxViewModel
    {
        private ICommand _openChildCommand;

        private ICommand _openModalCommand;

        private ICommand _openNavModalCommand;

        public ICommand OpenChildCommand
        {
            get
            {
                return _openChildCommand ?? (_openChildCommand = new MvxCommand(() => ShowViewModel<ChildViewModel>()));
            }
        }

        public ICommand OpenModalCommand
        {
            get
            {
                return _openModalCommand ?? (_openModalCommand = new MvxCommand(() => ShowViewModel<ModalViewModel>()));
            }
        }

        public ICommand OpenNavModalCommand
        {
            get
            {
                return _openNavModalCommand ?? (_openNavModalCommand =
                           new MvxCommand(() => ShowViewModel<ModalNavViewModel>()));
            }
        }
    }
}