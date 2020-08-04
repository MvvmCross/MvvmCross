using CoreGraphics;
using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
    public class MvxPopoverPresentationSourceProvider : IMvxPopoverPresentationSourceProvider
    {
        public CGRect PopoverSourceRect { get; }
        public UIView PopoverSourceView { get; }

        public MvxPopoverPresentationSourceProvider()
        {
        }

        public MvxPopoverPresentationSourceProvider(CGRect popoverSourceRect, UIView popoverSourceView)
        {
            PopoverSourceRect = popoverSourceRect;
            PopoverSourceView = popoverSourceView;
        }
    }
}
