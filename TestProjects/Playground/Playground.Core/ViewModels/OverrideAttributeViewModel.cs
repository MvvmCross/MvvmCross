using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class OverrideAttributeViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public OverrideAttributeViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            CloseCommand = new MvxAsyncCommand(async () => await _navigationService.CloseAsync(this));

            ShowTabsCommand = new MvxAsyncCommand(async () => await _navigationService.NavigateAsync<TabsRootViewModel>());
        }

        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }
    }
}
