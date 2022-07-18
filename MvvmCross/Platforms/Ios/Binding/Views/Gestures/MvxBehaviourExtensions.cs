// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Views.Gestures
{
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
