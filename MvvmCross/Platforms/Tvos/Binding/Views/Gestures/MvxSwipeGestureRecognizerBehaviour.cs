// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Views.Gestures
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
            };

            AddGestureRecognizer(target, swipe);
        }
    }
}
