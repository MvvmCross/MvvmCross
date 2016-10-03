// MvxBehaviourExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.tvOS.Views.Gestures
{
    using UIKit;

    public static class MvxBehaviourExtensions
    {
        public static MvxTapGestureRecognizerBehaviour Tap(this UIView view, uint numberOfTapsRequired = 1,
                                                           uint numberOfTouchesRequired = 1,
                                                           bool cancelsTouchesInView = true)
        {
            var toReturn = new MvxTapGestureRecognizerBehaviour(view, numberOfTapsRequired, numberOfTouchesRequired, cancelsTouchesInView);
            return toReturn;
        }

        public static MvxSwipeGestureRecognizerBehaviour Swipe(this UIView view, UISwipeGestureRecognizerDirection direction,
                                                               uint numberOfTouchesRequired = 1)
        {
            var toReturn = new MvxSwipeGestureRecognizerBehaviour(view, direction, numberOfTouchesRequired);
            return toReturn;
        }
    }
}