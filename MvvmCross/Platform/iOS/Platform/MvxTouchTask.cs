// MvxTouchTask.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.iOS.Platform
{
    using Foundation;

    using UIKit;

    public class MvxTouchTask
    {
        protected bool DoUrlOpen(NSUrl url)
        {
            return UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}