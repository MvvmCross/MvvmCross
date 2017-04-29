// MvxIosTask.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using UIKit;

namespace MvvmCross.Platform.iOS.Platform
{
    public class MvxIosTask
    {
        protected bool DoUrlOpen(NSUrl url)
        {
            var sharedApp = UIApplication.SharedApplication;
            return sharedApp.CanOpenUrl(url) && sharedApp.OpenUrl(url);
        }
    }
}