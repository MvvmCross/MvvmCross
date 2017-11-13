using CoreGraphics;
using UIKit;
using MvvmCross.Core.Views;

namespace MvvmCross.iOS.Views.Presenters.Attributes
{
    public class MvxModalPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = false;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;

        public static UIModalPresentationStyle DefaultModalPresentationStyle = UIModalPresentationStyle.FullScreen;
        public UIModalPresentationStyle ModalPresentationStyle { get; set; } = DefaultModalPresentationStyle;

        public static UIModalTransitionStyle DefaultModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
        public UIModalTransitionStyle ModalTransitionStyle { get; set; } = DefaultModalTransitionStyle;

        public static CGSize DefaultPreferredContentSize = CGSize.Empty;
        public CGSize PreferredContentSize { get; set; } = DefaultPreferredContentSize;

        public static bool DefaultAnimated = true;
        public bool Animated { get; set; } = DefaultAnimated;
    }
}
