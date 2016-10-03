// IMvxTvosModalHost.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.tvOS.Views
{
    using UIKit;

    public interface IMvxTvosModalHost
    {
        bool PresentModalViewController(UIViewController controller, bool animated);

        void NativeModalViewControllerDisappearedOnItsOwn();
    }
}