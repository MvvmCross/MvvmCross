using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Views.Attributes;

namespace Playground.Core.ViewModels
{
    public class MixedNavFirstViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        
        public MixedNavFirstViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public IMvxAsyncCommand LoginCommand => new MvxAsyncCommand(GotoMasterDetailPage, CanLogin);

        private bool CanLogin()
        {
            return true;
        }

        private async Task GotoMasterDetailPage()
        {
            await _navigationService.Navigate<MixedNavMasterDetailViewModel>();
            await _navigationService.Navigate<MixedNavMasterRootContentViewModel>();
        }
    }
}
