using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using TipCalc.Core.ViewModels;

namespace TipCalc.Core
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        private readonly IMvxNavigationService _mvxNavigationService;

        public AppStart(IMvxNavigationService mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;
        }

        public void Start(object hint = null)
        {
            _mvxNavigationService.Navigate<TipViewModel>();
        }


    }
}
