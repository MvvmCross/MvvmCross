using System;
using UIKit;

namespace MvvmCross.iOS.Views.Presenters.Attributes
{
    public class MvxModalPresentationAttribute : MvxBasePresentationAttribute
    {
        public bool WrapInNavigationController { get; set; }

        public UIModalPresentationStyle ModalPresentationStyle { get; set; }

        public UIModalTransitionStyle ModalTransitionStyle { get; set; }

        public bool Animated { get; set; } = true;
    }
}
