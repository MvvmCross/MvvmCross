using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Navigation
{
    public class ChangeTabBehaviorViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private bool _firstTime = true;

        public ChangeTabBehaviorViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private Task ShowInitialViewModels()
        {
            var tasks = new List<Task>
            {
                _navigationService.Navigate<FirstTabForBehaviorViewModel>(),
                _navigationService.Navigate<SecondTabForBehaviorViewModel>()
            };
            return Task.WhenAll(tasks);
        }

        public override void ViewAppearing()
        {
            if (_firstTime)
            {
                ShowInitialViewModels();
                _firstTime = false;
            }
        }
    }
}
