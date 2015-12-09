// MvxTapGestureRecognizerBehaviour.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views.Gestures
{
    public class MvxTapGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior<UITapGestureRecognizer>
    {
        protected override void HandleGesture(UITapGestureRecognizer gesture)
        {
            FireCommand();
        }

        public MvxTapGestureRecognizerBehaviour(UIView target, uint numberOfTapsRequired = 1,
                                                uint numberOfTouchesRequired = 1)
        {
            var tap = new UITapGestureRecognizer(HandleGesture)
            {
                NumberOfTapsRequired = numberOfTapsRequired,
                NumberOfTouchesRequired = numberOfTouchesRequired
            };

            AddGestureRecognizer(target, tap);
        }
    }
}