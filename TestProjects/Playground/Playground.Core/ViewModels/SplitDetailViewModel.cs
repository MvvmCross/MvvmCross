using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class SplitDetailViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public SplitDetailViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ChildViewModel>());
        }

        public IMvxAsyncCommand ShowChildCommand { get; private set; }
    }
}
