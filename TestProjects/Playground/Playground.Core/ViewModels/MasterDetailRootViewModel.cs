using System;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MasterDetailRootViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public MasterDetailRootViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public override async Task Initialize()
        {
            await _navigationService.Navigate<MasterDetailMasterViewModel>();
            await _navigationService.Navigate<MasterDetailDetail1ViewModel>();
        }
    }
}
