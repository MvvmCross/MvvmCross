// MvxGestureRecognizerBehavior.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Views.Gestures
{
    using System.Windows.Input;

    using UIKit;

    public abstract class MvxGestureRecognizerBehavior
    {
        public ICommand Command { get; set; }

        protected void FireCommand(object argument = null)
        {
            var command = this.Command;
            command?.Execute(null);
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
    public class MvxSwipeGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior<UISwipeGestureRecognizer>
    {
        public void Apply(UIView view, UISwipeGestureRecognizerDirection direction = UISwipeGestureRecognizerDirection.Right)
        {
            var tap = new UISwipeGestureRecognizer(FireCommandWithNull) {Direction = direction};
            view.AddGestureRecognizer(tap);
        }
    }

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