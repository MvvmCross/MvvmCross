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

            ShowChildCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<SplitDetailNavViewModel>());
        }

        public IMvxAsyncCommand ShowChildCommand { get; private set; }

        public string ContentText => "Text for the Content Area";

        public override void ViewAppeared()
        {
            base.ViewAppeared();
        }
    }
}
