using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Support.Core.ViewModels
{
    public class LeftPanelViewModel : BaseViewModel
    {      
        private MvxCommand showExampleMenuItemCommand;

        public MvxCommand ShowExampleMenuItemCommand
        {
            get
            {
                showExampleMenuItemCommand = showExampleMenuItemCommand ?? new MvxCommand(DoShowExampleMenuItem);
                return showExampleMenuItemCommand;
            }
        }

        private void DoShowExampleMenuItem()
        {
            ShowViewModel<ExampleMenuItemViewModel>();
        }

        private MvxCommand showMasterViewCommand;

        public MvxCommand ShowMasterViewCommand
        {
            get
            {
                showMasterViewCommand = showMasterViewCommand ?? new MvxCommand(ShowMasterView);
                return showMasterViewCommand;
            }
        }

        private void ShowMasterView()
        {
            ShowViewModel<MasterViewModel>();
        }
    }
}