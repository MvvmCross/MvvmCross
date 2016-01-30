// MvxTapGestureRecognizerBehaviour.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Views.Gestures
{
    using UIKit;

    public class MvxTapGestureRecognizerBehaviour
        : MvxGestureRecognizerBehavior<UITapGestureRecognizer>
    {
        protected override void HandleGesture(UITapGestureRecognizer gesture)
        {
            this.FireCommand();
        }

        public MvxTapGestureRecognizerBehaviour(UIView target, uint numberOfTapsRequired = 1,
                                                uint numberOfTouchesRequired = 1)
        {
            var tap = new UITapGestureRecognizer(this.HandleGesture)
            {
                NumberOfTapsRequired = numberOfTapsRequired,
#warning SHARED-APPLE: Missing member 'NumberOfTouchesRequired' (http://developer.xamarin.com/api/property/UIKit.UITapGestureRecognizer.NumberOfTouchesRequired/)
                //NumberOfTouchesRequired = numberOfTouchesRequired
            };

            this.AddGestureRecognizer(target, tap);
        }
    }
}