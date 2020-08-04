using MvvmCross;
using MvvmCross.Platforms.Ios.Presenters;
using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
    public class MvxPopoverDelegate : UIPopoverPresentationControllerDelegate
    {
        public MvxPopoverDelegate()
        {
        }
        
        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController forPresentationController)
        {
            return UIModalPresentationStyle.None;
        }

        public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController controller, UITraitCollection traitCollection)
        {
            return UIModalPresentationStyle.None;
        }

        public override void DidDismissPopover(UIPopoverPresentationController popoverPresentationController)
        {
            if (Mvx.IoCProvider.Resolve<IMvxIosViewPresenter>() is MvxIosViewPresenter presenter)
            {
                presenter.ClosedPopoverViewController();
            }
        }
    }
}
