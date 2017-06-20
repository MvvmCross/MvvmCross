using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoutingExample.Core.ViewModels;

namespace RoutingExample.Core
{
    public class AppStart : IMvxAppStart
    {
        private readonly IMvxNavigationService _navigationService;

        public AppStart(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void Start(object hint = null)
        {
            try
            {
                _navigationService.Navigate<MainViewModel>().GetAwaiter().GetResult();
            }
            catch (System.Exception e)
            {
            }
        }
    }
}
