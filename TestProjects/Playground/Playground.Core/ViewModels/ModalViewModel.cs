using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
namespace Playground.Core.ViewModels
{
    public class ModalViewModel : MvxViewModel
    {
        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new MvxCommand(() => this.Close(this)));
            }
        }
    }
}
