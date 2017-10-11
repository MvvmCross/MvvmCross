using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Playground.Core.ViewModels
{
    public class MixedNavFirstViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        
        public MixedNavFirstViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public ICommand LoginCommand => new MvxCommand(ExecuteLogin);

        private void ExecuteLogin()
        {
            if (CanLogin())
            {
                GotoMasterDetailPage();
            }
        }

        private bool CanLogin()
        {
            return true;
        }

        private void GotoMasterDetailPage()
        {
            _navigationService.Navigate<MixedNavMasterDetailViewModel>();
        }
    }
}
