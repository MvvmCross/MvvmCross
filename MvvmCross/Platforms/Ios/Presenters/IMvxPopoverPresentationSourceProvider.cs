using CoreGraphics;
using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
    public interface IMvxPopoverPresentationSourceProvider
    {
        UIView SourceView { get; set; }
        UIBarButtonItem SourceBarButtonItem { get; set; }
        public void SetSource(UIPopoverPresentationController popoverPresentationController);
    }
}
