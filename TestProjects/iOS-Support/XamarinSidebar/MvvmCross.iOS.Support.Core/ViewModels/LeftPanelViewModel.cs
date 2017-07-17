using System;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Support.XamarinSidebarSample.Core.ViewModels
{
    public class LeftPanelViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private MvxCommand _showExampleMenuItemCommand;
        private MvxCommand _showMasterViewCommand;

        public LeftPanelViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

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
            _navigationService.Navigate<ExampleMenuItemViewModel>();
        }

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
            _navigationService.Navigate<MasterViewModel>();
        }
    }
}