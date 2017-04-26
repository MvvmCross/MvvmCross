using MvvmCross.Core.ViewModels;
using Example.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Core.Navigation;

namespace Example.Core
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        /// <summary>
        /// Start is called on startup of the app
        /// Hint contains information in case the app is started with extra parameters
        /// </summary>
        public void Start(object hint = null)
        {
            Mvx.Resolve<IMvxNavigationService>().Navigate<LoginViewModel>();
			//ShowViewModel<LoginViewModel>();
        }
    }
}