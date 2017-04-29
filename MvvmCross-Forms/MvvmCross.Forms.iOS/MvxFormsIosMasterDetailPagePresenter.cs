using MvvmCross.Forms.Presenter.Core;
using MvvmCross.iOS.Views.Presenters;
using UIKit;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenter.iOS
{
    public class MvxFormsIosMasterDetailPagePresenter
        : MvxFormsMasterDetailPagePresenter
        , IMvxIosViewPresenter
    {
        private readonly UIWindow _window;

        public MvxFormsIosMasterDetailPagePresenter(UIWindow window, Application mvxFormsApp)
            : base(mvxFormsApp)
        {
            _window = window;
        }

        public virtual bool PresentModalViewController(UIViewController controller, bool animated)
        {
            return false;
        }

        public virtual void NativeModalViewControllerDisappearedOnItsOwn()
        {
        }

        protected override void CustomPlatformInitialization(MasterDetailPage mainPage)
        {
            _window.RootViewController = mainPage.CreateViewController();
        }
    }
}