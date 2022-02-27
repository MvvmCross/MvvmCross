// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Views.Gestures
{
    public abstract class MvxGestureRecognizerBehavior
    {
        public ICommand Command { get; set; }

        protected void FireCommand(object argument = null)
        {
            var command = Command;
            command?.Execute(argument);
        }

        protected void AddGestureRecognizer(UIView target, UIGestureRecognizer tap)
        {
            if (!target.UserInteractionEnabled)
                target.UserInteractionEnabled = true;

            target.AddGestureRecognizer(tap);
        }
    }

    public abstract class MvxGestureRecognizerBehavior<T>
        : MvxGestureRecognizerBehavior
    {
        protected virtual void HandleGesture(T gesture)
        {
        }
    }

    /*
     * these commented out as no use has been found for any of them yet
    public class MvxPinchGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior
    {
        public void Apply(UIView view)
        {
            var tap = new UIPinchGestureRecognizer(FireCommandWithNull);
            view.AddGestureRecognizer(tap);
        }
    }

    public class MvxPanGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior
    {
        public void Apply(UIView view, uint minTouches = 1, uint maxTouches = 1)
        {
            var tap = new UIPanGestureRecognizer(FireCommandWithNull)
                {
                    MaximumNumberOfTouches = maxTouches,
                    MinimumNumberOfTouches = minTouches
                };

            view.AddGestureRecognizer(tap);
        }
    }

    public class MvxRotationGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior
    {
        public void Apply(UIView view)
        {
            var tap = new UIRotationGestureRecognizer(FireCommandWithNull);
            view.AddGestureRecognizer(tap);
        }
    }
     */
}
