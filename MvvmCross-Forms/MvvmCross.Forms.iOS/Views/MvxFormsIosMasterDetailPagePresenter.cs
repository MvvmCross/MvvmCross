using MvvmCross.Forms.Views;
using MvvmCross.Forms.Platform;
using MvvmCross.iOS.Views.Presenters;
using UIKit;
using Xamarin.Forms;

namespace MvvmCross.Forms.iOS.Presenters
{
    public class MvxFormsIosMasterDetailPagePresenter
        : MvxFormsMasterDetailPagePresenter
        , IMvxIosViewPresenter
    {
        private readonly UIWindow _window;

        public MvxFormsIosMasterDetailPagePresenter(UIWindow window, MvxFormsApplication formsApplication)
            : base(formsApplication)
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