namespace MvvmCross.iOS.Support.XamarinSidebarSample.Core
{
    using MvvmCross.Core.ViewModels;
    using ViewModels;

    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        /// <summary>
        /// Start is called on startup of the app
        /// Hint contains information in case the app is started with extra parameters
        /// </summary>
        public void Start(object hint = null)
        {
            //ShowViewModel<LeftPanelViewModel>();
            ShowViewModel<RightPanelViewModel>();
            ShowViewModel<CenterPanelViewModel>();
        }
    }
}