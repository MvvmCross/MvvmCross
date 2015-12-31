// IMvxTouchModalHost.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.iOS.Views
{
    using UIKit;

    public interface IMvxTouchModalHost
    {
        bool PresentModalViewController(UIViewController controller, bool animated);

        void NativeModalViewControllerDisappearedOnItsOwn();
    }
}