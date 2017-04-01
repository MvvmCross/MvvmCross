using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
namespace Playground.Core.ViewModels
{
    public class ModalNavViewModel : MvxViewModel
    {
        public ModalNavViewModel()
        {
        }

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new MvxCommand(() => this.Close(this)));
            }
        }

        private ICommand _showChildCommand;
        public ICommand ShowChildCommand
        {
            get
            {
                return _showChildCommand ?? (_showChildCommand = new MvxCommand(() => this.ShowViewModel<ChildViewModel>()));
            }
        }
    }
}
