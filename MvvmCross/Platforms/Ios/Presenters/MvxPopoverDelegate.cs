using MvvmCross;
using MvvmCross.Platforms.Ios.Presenters;
using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
    public class MvxPopoverDelegate : UIPopoverPresentationControllerDelegate
    {
        private IMvxIosViewPresenter _presenter;
        public MvxPopoverDelegate(IMvxIosViewPresenter presenter)
        {
            _presenter = presenter;
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
            _presenter.ClosedPopoverViewController();
        }
    }
}
