using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Core.ViewModels
{
    public class IntroViewModel:MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public IntroViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public IMvxAsyncCommand GoMainPage => new MvxAsyncCommand(async () =>
          {
              await _navigationService.Navigate<MainViewViewModel>();
          });
    }
}
