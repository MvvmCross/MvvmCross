using System;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MasterDetailMasterViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public MasterDetailMasterViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            Detail1Command = new MvxAsyncCommand(async () => await _navigationService.Navigate<MasterDetailDetail1ViewModel>());
            Detail2Command = new MvxAsyncCommand(async () => await _navigationService.Navigate<MasterDetailDetail2ViewModel>());
        }

        public MvxAsyncCommand Detail1Command { get; }
        public MvxAsyncCommand Detail2Command { get; }
    }
}
