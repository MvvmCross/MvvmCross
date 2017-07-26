using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitMasterViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SplitMasterViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenDetailCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitDetailViewModel>());

            OpenDetailNavCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitDetailNavViewModel>());

            ShowRootViewModel = new MvxAsyncCommand(async () => await _navigationService.Navigate<RootViewModel>());
        }

        public IMvxAsyncCommand OpenDetailCommand { get; private set; }

        public IMvxAsyncCommand OpenDetailNavCommand { get; private set; }

        public IMvxAsyncCommand ShowRootViewModel { get; private set; }
    }
}
