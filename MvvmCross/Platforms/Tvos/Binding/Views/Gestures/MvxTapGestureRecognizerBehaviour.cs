// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Views.Gestures
{
    public class MvxTapGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior<UITapGestureRecognizer>
    {
        protected override void HandleGesture(UITapGestureRecognizer gesture)
        {
            FireCommand();
        }

        public MvxTapGestureRecognizerBehaviour(UIView target, uint numberOfTapsRequired = 1,
                                                uint numberOfTouchesRequired = 1,
                                                bool cancelsTouchesInView = true)
        {
            var tap = new UITapGestureRecognizer(HandleGesture)
            {
                NumberOfTapsRequired = numberOfTapsRequired,
                CancelsTouchesInView = cancelsTouchesInView
            };

            AddGestureRecognizer(target, tap);
        }
    }
}
