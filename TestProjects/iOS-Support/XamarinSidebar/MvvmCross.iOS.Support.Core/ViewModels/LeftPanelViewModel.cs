using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Support.XamarinSidebarSample.Core.ViewModels
{
    public class LeftPanelViewModel : BaseViewModel
    {
        private MvxCommand _showExampleMenuItemCommand;

        private MvxCommand _showMasterViewCommand;

        public MvxCommand ShowExampleMenuItemCommand
        {
            get
            {
                _showExampleMenuItemCommand = _showExampleMenuItemCommand ?? new MvxCommand(DoShowExampleMenuItem);
                return _showExampleMenuItemCommand;
            }
        }

        public MvxCommand ShowMasterViewCommand
        {
            get
            {
                _showMasterViewCommand = _showMasterViewCommand ?? new MvxCommand(ShowMasterView);
                return _showMasterViewCommand;
            }
        }

        private void DoShowExampleMenuItem()
        {
            ShowViewModel<ExampleMenuItemViewModel>();
        }

        private void ShowMasterView()
        {
            ShowViewModel<MasterViewModel>();
        }
    }
}