using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvxBindingsExample.ViewModels;

namespace MvxBindingsExample
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
                // Use this start to try the xaml bindings
                 _navigationService.Navigate<MainViewModel>().GetAwaiter().GetResult();

                // Use this start to try the code behind View & ViewModel
                //_navigationService.Navigate<CodeBehindViewModel>().GetAwaiter().GetResult();


                // Use this start to try the MvxFormsListview
                //_navigationService.Navigate<ListViewModel>().GetAwaiter().GetResult();
            }
            catch (System.Exception e)
            {
            }
        }
    }
}
