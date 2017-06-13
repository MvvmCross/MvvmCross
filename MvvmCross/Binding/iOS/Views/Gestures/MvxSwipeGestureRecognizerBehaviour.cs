// MvxSwipeGestureRecognizerBehaviour.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using UIKit;

namespace MvvmCross.Binding.iOS.Views.Gestures
{
    public class MvxSwipeGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior<UISwipeGestureRecognizer>
    {
        protected override void HandleGesture(UISwipeGestureRecognizer gesture)
        {
            FireCommand();
        }

        public MvxSwipeGestureRecognizerBehaviour(UIView target, UISwipeGestureRecognizerDirection direction,
                                                uint numberOfTouchesRequired = 1)
        {
            var swipe = new UISwipeGestureRecognizer(HandleGesture)
            {
                Direction = direction,
                NumberOfTouchesRequired = numberOfTouchesRequired
            };

            AddGestureRecognizer(target, swipe);
        }
    }
}