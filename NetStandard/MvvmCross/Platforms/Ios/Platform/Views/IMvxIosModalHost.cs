// IMvxIosModalHost.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using UIKit;

namespace MvvmCross.Platform.iOS.Views
{
    public interface IMvxIosModalHost
    {
        bool PresentModalViewController(UIViewController controller, bool animated);

        void NativeModalViewControllerDisappearedOnItsOwn();
    }
}