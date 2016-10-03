// MvxSwipeGestureRecognizerBehaviour.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Views.Gestures
{
    using UIKit;

    public class MvxSwipeGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior<UISwipeGestureRecognizer>
    {
        protected override void HandleGesture(UISwipeGestureRecognizer gesture)
        {
            this.FireCommand();
        }

        public MvxSwipeGestureRecognizerBehaviour(UIView target, UISwipeGestureRecognizerDirection direction,
                                                uint numberOfTouchesRequired = 1)
        {
            var swipe = new UISwipeGestureRecognizer(this.HandleGesture)
            {
                Direction = direction,
            };

            this.AddGestureRecognizer(target, swipe);
        }
    }
}