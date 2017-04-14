using System;
using UIKit;

namespace MvvmCross.iOS.Views.Presenters.Attributes
{
    public class MvxModalPresentationAttribute : MvxBasePresentationAttribute
    {
        public bool WrapInNavigationController { get; set; } = false;

        public UIModalPresentationStyle ModalPresentationStyle { get; set; } = UIModalPresentationStyle.FullScreen;

        public UIModalTransitionStyle ModalTransitionStyle { get; set; } = UIModalTransitionStyle.CoverVertical;

        public bool Animated { get; set; } = true;
    }
}
