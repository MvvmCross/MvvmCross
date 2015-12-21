using Cirrious.MvvmCross.ViewModels;
using Example.Core.ViewModels;

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
			ShowViewModel<HomeViewModel>();

			//This will show too soo on startup because the activity is not finished loading
			ShowViewModel<MenuViewModel>();
        }
    }
}