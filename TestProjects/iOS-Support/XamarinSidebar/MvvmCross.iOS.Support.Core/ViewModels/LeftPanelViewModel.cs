namespace MvvmCross.iOS.Support.XamarinSidebarSample.Core.ViewModels
{
    using MvvmCross.Core.ViewModels;

    public class LeftPanelViewModel : BaseViewModel
    {      
        private MvxCommand _showExampleMenuItemCommand;

        public MvxCommand ShowExampleMenuItemCommand
        {
            get
            {
                _showExampleMenuItemCommand = _showExampleMenuItemCommand ?? new MvxCommand(DoShowExampleMenuItem);
                return _showExampleMenuItemCommand;
            }
        }

        private void DoShowExampleMenuItem()
        {
            ShowViewModel<ExampleMenuItemViewModel>();
        }

        private MvxCommand _showMasterViewCommand;

        public MvxCommand ShowMasterViewCommand
        {
            get
            {
                _showMasterViewCommand = _showMasterViewCommand ?? new MvxCommand(ShowMasterView);
                return _showMasterViewCommand;
            }
        }

        private void ShowMasterView()
        {
            ShowViewModel<MasterViewModel>();
        }
    }
}