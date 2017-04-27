using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class TabsRootViewModel : MvxViewModel
    {
        private ICommand _showInitialViewModelsCommand;
        public ICommand ShowInitialViewModelsCommand
        {
            get
            {
                return _showInitialViewModelsCommand ?? (_showInitialViewModelsCommand = new MvxCommand(ShowInitialViewModels));
            }
        }

        private void ShowInitialViewModels()
        {
            ShowViewModel<Tab1ViewModel>();
            ShowViewModel<Tab2ViewModel>();
        }
    }
}
