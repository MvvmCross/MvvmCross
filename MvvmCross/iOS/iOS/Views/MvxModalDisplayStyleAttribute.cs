using System;
using UIKit;

namespace MvvmCross.iOS.Views
{
    public class MvxModalDisplayStyleAttribute : Attribute
    {
        public UIModalPresentationStyle? ModalPresentationStyle { get; private set; }

        public UIModalTransitionStyle? ModalTransitionStyle { get; private set; }

        public bool Animated { get; private set; }

        public MvxModalDisplayStyleAttribute(UIModalPresentationStyle modalPresentationStyle, bool animated = true)
        {
            ModalPresentationStyle = modalPresentationStyle;
            Animated = animated;
        }

        public MvxModalDisplayStyleAttribute(UIModalTransitionStyle modalTransitionStyle, bool animated = true)
        {
            ModalTransitionStyle = modalTransitionStyle;
            Animated = animated;
        }

        public MvxModalDisplayStyleAttribute(
            UIModalPresentationStyle modalPresentationStyle,
            UIModalTransitionStyle modalTransitionStyle,
            bool animated = true)
        {
            ModalPresentationStyle = modalPresentationStyle;
            ModalTransitionStyle = modalTransitionStyle;
            Animated = animated;
        }
    }
}
