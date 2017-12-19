// MvxTvosTask.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Foundation;
using UIKit;

namespace MvvmCross.Platform.tvOS.Platform
{
    public class MvxTvosTask
    {
        protected bool DoUrlOpen(NSUrl url)
        {
            return UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}