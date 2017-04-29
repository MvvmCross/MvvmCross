// MvxSwipeGestureRecognizerBehaviour.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using UIKit;

namespace MvvmCross.Binding.tvOS.Views.Gestures
{
    public class MvxSwipeGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior<UISwipeGestureRecognizer>
    {
        public MvxSwipeGestureRecognizerBehaviour(UIView target, UISwipeGestureRecognizerDirection direction,
            uint numberOfTouchesRequired = 1)
        {
            var swipe = new UISwipeGestureRecognizer(HandleGesture)
            {
                Direction = direction
            };

            AddGestureRecognizer(target, swipe);
        }

        protected override void HandleGesture(UISwipeGestureRecognizer gesture)
        {
            FireCommand();
        }
    }
}