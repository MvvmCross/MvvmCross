using System;
using UIKit;

namespace MvvmCross.iOS.Views.Presenters.Attributes
{
    public class MvxModalPresentationAttribute : MvxBasePresentationAttribute
    {
        public bool WrapInNavigationController { get; set; }

        public UIModalPresentationStyle ModalPresentationStyle { get; private set; }

        public UIModalTransitionStyle ModalTransitionStyle { get; private set; }

        public bool Animated { get; private set; }
    }
}
