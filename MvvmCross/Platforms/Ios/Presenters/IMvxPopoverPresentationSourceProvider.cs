using CoreGraphics;
using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
    public interface IMvxPopoverPresentationSourceProvider
    {
        CGRect PopoverSourceRect { get; }
        UIView PopoverSourceView { get; }
    }
}
